using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Common.Exceptions
{
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException(int accountId)
            : base($"На счету с ID {accountId} недостаточно средств для выполнения операции.")
        {
        }
    }
}
