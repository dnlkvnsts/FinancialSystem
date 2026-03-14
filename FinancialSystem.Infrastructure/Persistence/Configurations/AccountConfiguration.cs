using FinancialSystem.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialSystem.Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            // Указываем название таблицы (по желанию, по умолчанию будет "Account")
            builder.ToTable("Accounts");

            // Первичный ключ (Id наследуется из AggregateRoot/Entity)
            builder.HasKey(t => t.Id);

            // Настройка номера счета
            builder.Property(t => t.Number)
                .HasMaxLength(34) // Например, длина IBAN
                .IsRequired();

            // Настройка владельца счета
            builder.Property(t => t.OwnerId)
                .IsRequired();

            // Настройка флага блокировки
            builder.Property(t => t.IsBlocked)
                .IsRequired()
                .HasDefaultValue(false); // По умолчанию счет не заблокирован

            // Настройка Value Object (Деньги)
            builder.OwnsOne(t => t.Balance, moneyBuilder =>
            {
                moneyBuilder.Property(m => m.Amount)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired()
                    .HasColumnName("BalanceAmount");

                moneyBuilder.Property(m => m.Currency)
                    .HasMaxLength(3)
                    .IsRequired()
                    .HasColumnName("BalanceCurrency");
            });

            // ВАЖНО ДЛЯ DDD: 
            // Говорим EF Core ИГНОРИРОВАТЬ список доменных событий при сохранении в БД.
            // (Предполагаю, что в твоем базовом классе AggregateRoot есть коллекция типа DomainEvents)
            builder.Ignore(t => t.DomainEvents);
        }
    }
}
