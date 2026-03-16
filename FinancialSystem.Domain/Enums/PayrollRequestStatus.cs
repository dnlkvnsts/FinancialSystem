using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Domain.Enums
{
    public enum PayrollRequestStatus
    {
        Pending = 1,   // Ожидает подтверждения менеджера
        Approved = 2,  // Одобрено (можно получать зарплату)
        Rejected = 3,   // Отклонено
        Completed = 4
    }
}
