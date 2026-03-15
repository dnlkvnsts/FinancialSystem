using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Domain.Aggregates;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Domain.ValueObjects;
using MediatR;


namespace FinancialSystem.Application.Features.Accounts.Commands.OpenAccount
{
    public class OpenAccountCommandHandler : IRequestHandler<OpenAccountCommand, int>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OpenAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(OpenAccountCommand request, CancellationToken cancellationToken)
        {

            if (request.Type == AccountType.Savings && request.InterestRate <= 0)
            {
                throw new Exception("Для открытия вклада процентная ставка должна быть больше 0.");
            }


            if (request.Type == AccountType.Debit && request.InterestRate > 0)
            {
                throw new Exception("У обычного счета (Debit) не может быть процентной ставки.");
            }
            // 1. Генерируем уникальный номер счета (бизнес-логика)
            string newAccountNumber = "BY" + Guid.NewGuid().ToString().Substring(0, 10).ToUpper();

            // 2. Создаем начальный баланс (Value Object Money)
            var initialBalance = new Money(request.Currency, 0m);

            // 3. Создаем сущность Account (используем тот самый конструктор с 4 параметрами)
            var account = new Account(
                newAccountNumber,
                initialBalance,
                request.OwnerId,
                request.BankId,
                request.Type,
                 request.InterestRate
            );

            // 4. Добавляем в репозиторий и сохраняем
            await _accountRepository.AddAsync(account);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 5. Возвращаем ID нового счета
            return account.Id;
        }
    }

}
