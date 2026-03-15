using FinancialSystem.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Accounts.Commands.OpenAccount
{
    public record OpenAccountCommand : IRequest<int>
    {
        public int BankId { get; init; }      // В каком банке открываем
        public string Currency { get; init; } // "BYN", "USD", "RUB"
        public int OwnerId { get; init; }     // Кто владелец

        
        public AccountType Type { get; init; }

        public decimal InterestRate { get; init; } = 0;
    }
}
