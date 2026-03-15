using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.DTOs
{
    public class TransferResponseDto
    {
        public int TransactionId { get; set; }        // ID созданной транзакции
        public decimal NewBalance { get; set; }        // Новый баланс отправителя
        public string Currency { get; set; }           // Валюта перевода
        public string Message { get; set; }            // Сообщение (напр. "Успешно")
        public DateTime TransactionDate { get; set; }  // Дата и время
    }
}
