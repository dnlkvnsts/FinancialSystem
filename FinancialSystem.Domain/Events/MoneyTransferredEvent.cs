using FinancialSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Domain.Events
{
    public record MoneyTransferredEvent(
        int PayrollRequestId,    // ID заявки, по которой платим
        int BankId,              // ID выбранного банка
        string TargetAccountNumber, // Номер счета (может быть внешним)
        decimal Amount           // Сумма перевода
    ) : IDomainEvent
    {
        public DateTime OccurredOn => DateTime.UtcNow;
    }
}
