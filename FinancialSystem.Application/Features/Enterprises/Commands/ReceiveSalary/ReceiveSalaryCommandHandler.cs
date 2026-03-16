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

            // --- НОВАЯ ЛОГИКА: ПРИСОЕДИНЕНИЕ К ПРЕДПРИЯТИЮ ---
            // 3. Загружаем предприятие, к которому относится эта заявка
            var enterprise = await _context.Enterprises
                .FirstOrDefaultAsync(e => e.Id == payrollRequest.EnterpriseId, cancellationToken);

            if (enterprise != null)
            {
                // Добавляем ID клиента в список сотрудников предприятия
                enterprise.AddEmployee(payrollRequest.ClientId);
            }
            // ------------------------------------------------

            // 4. ИЩЕМ СЧЕТ КЛИЕНТА (чтобы изменить баланс)
            var targetAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Number == request.AccountNumber
                                       && a.BankId == request.BankId, cancellationToken);

            if (targetAccount == null)
                throw new Exception("Указанный счет не найден в этом банке.");

            // 5. ЗАЧИСЛЯЕМ ДЕНЬГИ
            var moneyToDeposit = new Money(targetAccount.Balance.Currency, request.Amount);
            targetAccount.Deposit(moneyToDeposit);

            // 6. Обновляем статус заявки
            payrollRequest.MarkAsReceived(request.BankId, request.AccountNumber);

            // 7. Сохраняем ВСЁ одновременно:
            // - Измененный баланс счета (targetAccount)
            // - Новый статус заявки (payrollRequest)
            // - Обновленный список сотрудников предприятия (enterprise)
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
