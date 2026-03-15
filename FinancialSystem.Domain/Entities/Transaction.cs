using FinancialSystem.Domain.Aggregates;
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
        public int? FromAccountId { get; private set; }
        public int? ToAccountId { get; private set; }

        // === ДОБАВЬ ЭТИ ДВЕ СТРОКИ ЗДЕСЬ ===
        public virtual Account FromAccount { get; private set; } // Навигационное свойство
        public virtual Account ToAccount { get; private set; }   // Навигационное свойство
                                                                 // ==================================

        public Money Amount { get; private set; }
        public TransactionType Type { get; private set; }
        public string Description { get; private set; }

        private Transaction() { } // Для EF

        public Transaction(int? fromAccountId, int? toAccountId, Money amount, TransactionType type, string description = "")
        {
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Amount = amount;
            Type = type;
            Description = description;
        }
    }
}
