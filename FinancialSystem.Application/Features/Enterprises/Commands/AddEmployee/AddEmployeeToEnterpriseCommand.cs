using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Enterprises.Commands.AddEmployee
{
    public record AddEmployeeToEnterpriseCommand(int EnterpriseId, int ClientId) : IRequest;
}
