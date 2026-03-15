using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Accounts.Queries.GetAccountHistory
{
    public class GetAccountHistoryQueryHandler : IRequestHandler<GetAccountHistoryQuery, List<TransactionDto>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;

        public GetAccountHistoryQueryHandler(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }

        public async Task<List<TransactionDto>> Handle(GetAccountHistoryQuery request, CancellationToken cancellationToken)
        {
            // 1. Получаем транзакции из репозитория
            // Фильтруем: где AccountId == FromAccountId ИЛИ AccountId == ToAccountId
            var transactions = await _transactionRepository.GetByAccountIdAsync(request.AccountId);

            // 2. Маппим во фронтенд-модель (DTO)
            return transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                FromAccountNumber = t.FromAccount != null ? t.FromAccount.Number : "Внешний счет",
                ToAccountNumber = t.ToAccount != null ? t.ToAccount.Number : "Внешний счет",
                Amount = t.Amount.Amount,
                Currency = t.Amount.Currency,
                Description = t.Description,
                CreatedAt = t.CreatedAt,
                TransactionType = t.Type.ToString()
            }).OrderByDescending(t => t.CreatedAt).ToList(); // Сначала новые
        }
    }

}
