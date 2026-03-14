using FinancialSystem.Application.Common.Exceptions;
using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Application.DTOs;
using FinancialSystem.Domain.Aggregates;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Accounts.Queries.GetAccountById
{
    public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountDto>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountByIdQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountDto> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            // 1. Ищем счет в базе
            var account = await _accountRepository.GetByIdAsync(request.AccountId);

            // 2. Если не нашли — выбрасываем нашу красивую ошибку, которую создали на прошлом шаге!
            if (account == null)
            {
                throw new NotFoundException(nameof(Account), request.AccountId);
            }

            // 3. Мапим (преобразуем) сущность БД в DTO для отправки пользователю
            return new AccountDto
            {
                Id = account.Id,
                UserId = account.OwnerId, // У тебя поле называется OwnerId
                Balance = account.Balance.Amount // Достаем Amount из объекта Money
            };
        }
    }
}
