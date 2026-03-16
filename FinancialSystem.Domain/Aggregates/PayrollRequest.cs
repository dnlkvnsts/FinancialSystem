using FinancialSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Domain.Aggregates
{
    public class PayrollRequest
    {
        public int Id { get; private set; }
        public int ClientId { get; private set; } // Кто подает
        public int EnterpriseId { get; private set; } // От какого предприятия
        public PayrollRequestStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }



        public int? TargetBankId { get; private set; }
        public string? TargetAccountNumber { get; private set; }

        // Пустой конструктор для Entity Framework
        protected PayrollRequest() { }

        // Конструктор для создания новой заявки клиентом
        public PayrollRequest(int clientId, int enterpriseId)
        {
            ClientId = clientId;
            EnterpriseId = enterpriseId;
            Status = PayrollRequestStatus.Pending; // По умолчанию ждет ответа
            CreatedAt = DateTime.UtcNow;
        }

        // Методы для Менеджера (инкапсуляция бизнес-логики)
        public void Approve()
        {
            Status = PayrollRequestStatus.Approved;
        }

        public void Reject()
        {
            Status = PayrollRequestStatus.Rejected;
        }

        public void UpdateStatus(PayrollRequestStatus newStatus)
        {
            Status = newStatus;
        }

        public void MarkAsReceived(int bankId, string accountNumber)
        {
            // БИЗНЕС-ПРАВИЛО: Получить деньги можно только если заявка ОДОБРЕНА менеджером
            if (Status != PayrollRequestStatus.Approved)
            {
                throw new Exception("Нельзя получить средства по заявке, которая не была одобрена.");
            }

            TargetBankId = bankId;
            TargetAccountNumber = accountNumber;
            Status = PayrollRequestStatus.Completed; // Устанавливаем финальный статус
        }

    }
}
