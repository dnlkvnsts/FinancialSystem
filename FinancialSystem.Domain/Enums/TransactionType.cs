using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Domain.Enums
{
    public enum TransactionType
    {
        Transfer,      // Обычный перевод
        Salary,        // Зарплата от предприятия
        DepositInterest, // Проценты по вкладу
        Withdrawal,    // Снятие
        Refill         // Пополнение
    }
}
