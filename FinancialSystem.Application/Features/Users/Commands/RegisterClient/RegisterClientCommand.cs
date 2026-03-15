using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Users.Commands.RegisterClient
{
    public record RegisterClientCommand(
    string FullName,
    string Email,
    string Password) : IRequest<int>;


}
