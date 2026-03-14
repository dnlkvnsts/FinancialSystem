using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Common.Interfaces
{
    public interface IDateTime
    {
        // Текущее время системы
        DateTime Now { get; }
    }
}
