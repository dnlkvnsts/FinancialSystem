using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Accounts.Commands.UpdateAccountLockStatus
{
    public record UpdateAccountLockStatusCommand(int AccountId, bool ShouldBlock) : IRequest;
}
