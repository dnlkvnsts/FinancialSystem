using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Accounts.Commands.AccrueInterest
{
    public record AccrueInterestCommand(int AccountId) : IRequest;
}
