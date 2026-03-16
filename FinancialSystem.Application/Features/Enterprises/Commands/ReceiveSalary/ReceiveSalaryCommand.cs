using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Enterprises.Commands.ReceiveSalary
{
    public record ReceiveSalaryCommand(
    int RequestId,
    int BankId,
    string AccountNumber,
    int ClientId,
    decimal Amount // <--- Добавили сумму здесь (только для запроса)
) : IRequest<bool>;

}
