using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Users.Commands.ConfirmClient
{
    public record ConfirmClientCommand(int UserId) : IRequest;
}
