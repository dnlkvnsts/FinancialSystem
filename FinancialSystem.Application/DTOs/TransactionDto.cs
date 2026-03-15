using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string FromAccountNumber { get; set; } // Откуда
        public string ToAccountNumber { get; set; }   // Куда
        public decimal Amount { get; set; }           // Сколько
        public string Currency { get; set; }          // Валюта
        public string Description { get; set; }       // Описание
        public DateTime CreatedAt { get; set; }       // Когда
        public string TransactionType { get; set; }   // Тип (Перевод, Пополнение и т.д.)
    }
}
