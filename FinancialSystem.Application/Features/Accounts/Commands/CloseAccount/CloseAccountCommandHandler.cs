using FinancialSystem.Application.Common.Exceptions;
using FinancialSystem.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Accounts.Commands.CloseAccount
{
    public class CloseAccountCommandHandler : IRequestHandler<CloseAccountCommand, Unit>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CloseAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
        {
            // 1. Ищем счет в базе
            var account = await _accountRepository.GetByIdAsync(request.AccountId);

            if (account == null)
                throw new NotFoundException("Счет не найден");

            // 2. Вызываем бизнес-логику закрытия (проверка баланса и смена флага)
            account.Close();

            // 3. Сохраняем изменения
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
