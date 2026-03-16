using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Enterprises.Commands.ReceiveSalary
{
    public class ReceiveSalaryCommandHandler : IRequestHandler<ReceiveSalaryCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public ReceiveSalaryCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task<bool> Handle(ReceiveSalaryCommand request, CancellationToken cancellationToken)
        {
            // 1. Ищем заявку в базе
            var payrollRequest = await _context.PayrollRequests
                .FirstOrDefaultAsync(x => x.Id == request.RequestId, cancellationToken);

            if (payrollRequest == null)
                throw new Exception("Заявка не найдена.");

            // 2. БЕЗОПАСНОСТЬ: Проверяем владельца
            if (payrollRequest.ClientId != request.ClientId)
                throw new Exception("У вас нет прав на получение средств по этой заявке.");

            // 3. ИЩЕМ СЧЕТ КЛИЕНТА (чтобы изменить баланс)
            var targetAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Number == request.AccountNumber
                                       && a.BankId == request.BankId, cancellationToken);

            if (targetAccount == null)
                throw new Exception("Указанный счет не найден в этом банке.");

            // 4. ЗАЧИСЛЯЕМ ДЕНЬГИ (используем Amount из запроса и валюту из счета)
            var moneyToDeposit = new Money(targetAccount.Balance.Currency, request.Amount);
            targetAccount.Deposit(moneyToDeposit);

            // 5. Вызываем твой метод для обновления статуса заявки
            payrollRequest.MarkAsReceived(request.BankId, request.AccountNumber);

            // 6. Сохраняем изменения (и баланс счета, и статус заявки сохранятся одновременно)
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
