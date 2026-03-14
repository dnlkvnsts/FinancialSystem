using FinancialSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Domain.Events
{
    public record MoneyTransferredEvent(int AccountId, decimal Amount) : IDomainEvent
    {
        public DateTime OccurredOn => DateTime.UtcNow;
    }
}
