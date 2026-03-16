using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Admin.Commands.UndoAction
{
    public record UndoActionCommand(int LogId) : IRequest<Unit>;
}
