using FinancialSystem.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Enterprises.Commands.UpdatePayrollRequestStatus
{
    public record UpdatePayrollRequestStatusCommand : IRequest<Unit>
    {
        public int Id { get; init; }
        public PayrollRequestStatus Status { get; init; }
    }
}
