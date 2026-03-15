using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Domain.Exceptions;
using FinancialSystem.Domain.ValueObjects;
using MediatR;


namespace FinancialSystem.Application.Features.Accounts.Commands.AccrueInterest
{
    public class AccrueInterestCommandHandler : IRequestHandler<AccrueInterestCommand>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AccrueInterestCommandHandler(
            IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AccrueInterestCommand request, CancellationToken cancellationToken)
        {
            // 1. Получаем вклад
            var account = await _accountRepository.GetByIdAsync(request.AccountId);

            if (account == null)
                throw new DomainException("Вклад не найден");

            // Запоминаем баланс до начисления для описания
            decimal oldBalance = account.Balance.Amount;

            // 2. Вызываем логику начисления процентов в сущности
            account.AccrueInterest();

            // Рассчитываем сумму начисленных процентов
            decimal accruedAmount = account.Balance.Amount - oldBalance;

            if (accruedAmount > 0)
            {
                // 3. Регистрируем транзакцию
                var transaction = new Transaction(
                    fromAccountId: null, // Для банка это расход, для клиента — доход извне
                    toAccountId: account.Id,
                    amount: new Money(account.Balance.Currency, accruedAmount),
                    type: TransactionType.Interest, // Добавь Interest в свой Enum TransactionType
                    description: $"Ежемесячное начисление процентов по ставке {account.InterestRate}%"
                );

                await _transactionRepository.AddAsync(transaction);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
