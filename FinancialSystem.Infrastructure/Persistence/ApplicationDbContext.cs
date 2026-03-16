using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Domain.Aggregates;
using FinancialSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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

            // 1. Настройка для Transaction (исправляет твою ошибку)
            builder.Entity<Transaction>(entity =>
            {
                // Указываем, что Amount — это часть таблицы Transaction
                entity.OwnsOne(t => t.Amount, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("Amount").HasPrecision(18, 2);
                    money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3);
                });
            });

            // 2. Настройка для Account (так как там тоже есть Money)
            builder.Entity<Account>(entity =>
            {
                entity.OwnsOne(a => a.Balance, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("BalanceAmount").HasPrecision(18, 2);
                    money.Property(m => m.Currency).HasColumnName("BalanceCurrency").HasMaxLength(3);
                });
            });

            // Оставляем эту строку на случай, если в будущем захочешь использовать файлы
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
