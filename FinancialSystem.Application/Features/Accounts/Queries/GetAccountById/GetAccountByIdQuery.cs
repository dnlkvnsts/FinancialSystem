using FinancialSystem.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Accounts.Queries.GetAccountById
{
    public record GetAccountByIdQuery(int AccountId) : IRequest<AccountDto>;
}