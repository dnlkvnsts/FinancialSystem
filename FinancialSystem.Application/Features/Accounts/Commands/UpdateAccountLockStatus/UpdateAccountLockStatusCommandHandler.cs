using FinancialSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Accounts.Commands.UpdateAccountLockStatus
{
    public class UpdateAccountLockStatusHandler : IRequestHandler<UpdateAccountLockStatusCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateAccountLockStatusHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateAccountLockStatusCommand request, CancellationToken cancellationToken)
        {
            // 1. Находим счет в базе
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == request.AccountId, cancellationToken);

            if (account == null)
                throw new Exception("Счет не найден");

            // 2. Вызываем доменный метод в зависимости от команды
            if (request.ShouldBlock)
            {
                account.Block();
            }
            else
            {
                account.Unblock();
            }

            // 3. Сохраняем изменения
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}
