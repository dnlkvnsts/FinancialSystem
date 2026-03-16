using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.DTOs
{
    public class ReceiveSalaryDto
    {
        public int BankId { get; set; }
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; } // <--- ДОБАВЬ ЭТО ПОЛЕ
    }
}
