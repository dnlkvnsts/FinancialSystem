using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Domain.Aggregates;
using FinancialSystem.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, int>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            // 1. Создаем начальный баланс (0). Порядок: валюта, затем сумма.
            var initialBalance = new Money(request.Currency, 0m);

            // 2. Создаем сущность Account через конструктор, как ты и задумал
            var account = new Account(request.AccountNumber, initialBalance, request.OwnerId);

            // 3. Добавляем в репозиторий
            await _accountRepository.AddAsync(account);

            // 4. Сохраняем изменения в базу (через Unit of Work)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 5. Возвращаем ID нового счета (свойство Id наследуется от AggregateRoot)
            return account.Id;
        }
    }
}
