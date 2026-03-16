using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Domain.Aggregates;
using FinancialSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinancialSystem.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext, IUnitOfWork
    {
        private IDbContextTransaction? _currentTransaction;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // РЕАЛИЗАЦИЯ IApplicationDbContext (Все DbSet)
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Bank> Banks => Set<Bank>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<Enterprise> Enterprises  => Set<Enterprise>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

       public DbSet<PayrollRequest>  PayrollRequests => Set<PayrollRequest>();

        // РЕАЛИЗАЦИЯ IUnitOfWork (Управление транзакциями)
        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null) return;
            _currentTransaction = await Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                if (_currentTransaction != null) await _currentTransaction.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_currentTransaction != null) await _currentTransaction.RollbackAsync();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        // Стандартный метод сохранения
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. Настройка для Transaction
            builder.Entity<Transaction>(entity =>
            {
                entity.OwnsOne(t => t.Amount, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("Amount").HasPrecision(18, 2);
                    money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3);
                });
            });

            // 2. Настройка для Account
            builder.Entity<Account>(entity =>
            {
                entity.OwnsOne(a => a.Balance, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("BalanceAmount").HasPrecision(18, 2);
                    money.Property(m => m.Currency).HasColumnName("BalanceCurrency").HasMaxLength(3);
                });
            });

            // --- НОВАЯ НАСТРОЙКА ДЛЯ ENTERPRISE (КОЛОНКА С ПОЛЬЗОВАТЕЛЯМИ) ---
            builder.Entity<Enterprise>(entity =>
            {
                entity.Property(e => e.EmployeeIds)
                    .HasField("_employeeIds")
                    .UsePropertyAccessMode(PropertyAccessMode.Field)
                    .HasConversion(
                        // При сохранении: если списка нет, сохраняем "[]"
                        v => JsonSerializer.Serialize(v ?? new List<int>(), (JsonSerializerOptions)null),

                        // При чтении: ПРОВЕРЯЕМ НА ПУСТОТУ перед тем как десериализовать
                        v => string.IsNullOrEmpty(v)
                            ? new List<int>()
                            : JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions)null) ?? new List<int>()
                    );

                // Оставляем компаратор без изменений
                var comparer = new ValueComparer<IReadOnlyCollection<int>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList());

                entity.Property(e => e.EmployeeIds).Metadata.SetValueComparer(comparer);
            });
            // ----------------------------------------------------------------

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
