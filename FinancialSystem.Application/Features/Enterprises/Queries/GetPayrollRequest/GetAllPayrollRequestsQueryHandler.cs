using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Enterprises.Queries.GetPayrollRequest
{
    public class GetAllPayrollRequestsQueryHandler : IRequestHandler<GetAllPayrollRequestsQuery, List<PayrollRequestDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllPayrollRequestsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PayrollRequestDto>> Handle(GetAllPayrollRequestsQuery request, CancellationToken cancellationToken)
        {
            return await _context.PayrollRequests
                .Select(r => new PayrollRequestDto
                {
                    Id = r.Id,
                    EnterpriseId = r.EnterpriseId,
                    EnterpriseName = "Enterprise #" + r.EnterpriseId, // Заглушка, так как связи нет
                    Status = (int)r.Status,
                    // Если в самой базе в PayrollRequest тоже нет CreatedAt, удали эту строку и из DTO
                    CreatedAt = DateTime.UtcNow
                })
                .ToListAsync(cancellationToken);
        }
    }
}
