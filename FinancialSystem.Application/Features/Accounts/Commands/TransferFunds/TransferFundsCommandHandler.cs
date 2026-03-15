using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Entities;
using FinancialSystem.Domain.Enums;
using FinancialSystem.Domain.Exceptions;
using FinancialSystem.Domain.ValueObjects;
using MediatR;


namespace FinancialSystem.Application.Features.Accounts.Commands.TransferFunds
{
    public class TransferFundsCommandHandler : IRequestHandler<TransferFundsCommand, TransferResponseDto>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransferFundsCommandHandler(
            IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransferResponseDto> Handle(TransferFundsCommand request, CancellationToken cancellationToken)
        {
            // 1. Поиск счетов
            var fromAccount = await _accountRepository.GetByIdAsync(request.FromAccountId);
            var toAccount = await _accountRepository.GetByIdAsync(request.ToAccountId);

            if (fromAccount == null) throw new DomainException("Счет отправителя не найден");
            if (toAccount == null) throw new DomainException("Счет получателя не найден");

            // 2. ОПРЕДЕЛЯЕМ, КУДА ИДЕТ ПЕРЕВОД (На счет или на вклад)
            // Мы понимаем это автоматически по типу toAccount
            string destinationType = toAccount.Type == AccountType.Savings ? "вклад" : "текущий счет";

            // 3. Создаем объект Money
            var transferAmount = new Money(request.Currency, request.Amount);

            // 4. Выполняем операции (бизнес-логика внутри Account проверит, не закрыт ли счет)
            fromAccount.Withdraw(transferAmount);
            toAccount.Deposit(transferAmount);

            // 5. Создаем запись транзакции с информативным описанием
            var transaction = new Transaction(
                fromAccountId: fromAccount.Id,
                toAccountId: toAccount.Id,
                amount: transferAmount,
                type: TransactionType.Transfer, // Используем твой Enum
                description: $"Перевод на {destinationType}. Со счета {fromAccount.Number} на {toAccount.Number}"
            );

            // 6. Сохранение (Atomic operation через UnitOfWork)
            await _transactionRepository.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 7. Формируем ответ
            return new TransferResponseDto
            {
                TransactionId = transaction.Id,
                NewBalance = fromAccount.Balance.Amount,
                Currency = fromAccount.Balance.Currency,
                Message = $"Перевод на {destinationType} успешно выполнен",
                TransactionDate = transaction.CreatedAt
            };
        }
    }
}
