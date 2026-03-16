using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Enterprises.Commands.CreatePayrollRequest
{
    public record CreatePayrollRequestCommand(int ClientId, int EnterpriseId) : IRequest<int>;
}
