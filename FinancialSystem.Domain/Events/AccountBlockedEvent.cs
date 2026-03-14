using FinancialSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Domain.Events
{
    public record AccountBlockedEvent(int AccountId, string Reason) : IDomainEvent
    {
        public DateTime OccurredOn => DateTime.UtcNow;
    }
}
