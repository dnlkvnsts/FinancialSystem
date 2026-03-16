using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Enterprises.Commands.RemoveEmployee
{
    public record RemoveEmployeeFromEnterpriseCommand(int EnterpriseId, int ClientId) : IRequest;
}
