using FinancialSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Domain.Events
{

    public record UserRegisteredEvent(int UserId, string Email) : IDomainEvent
    {
        // Событие само фиксирует время, когда оно было создано
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
