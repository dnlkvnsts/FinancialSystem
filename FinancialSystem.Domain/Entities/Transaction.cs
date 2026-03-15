using FinancialSystem.Domain.Common;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Domain.Entities
{
    public class Transaction : Entity
    {
        public int? FromAccountId { get; private set; } // int? (может быть null, если это внешнее пополнение)
        public int? ToAccountId { get; private set; }

        // Используем Money для консистентности с Account
        public Money Amount { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public TransactionType Type { get; private set; } // Перевод, Зарплата, Проценты и т.д.
        public string Description { get; private set; }

        private Transaction() { } // Для EF

        public Transaction(int? fromAccountId, int? toAccountId, Money amount, TransactionType type, string description = "")
        {
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Amount = amount;
            Type = type;
            Description = description;
            CreatedAt = DateTime.UtcNow; // Фиксируем время создания
        }
    }
}
