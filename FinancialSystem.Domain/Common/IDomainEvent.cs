using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}

