using AutoMapper;
using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using FinancialSystem.Domain.Aggregates;

namespace FinancialSystem.Application.Features.Enterprises.Queries.GetEnterprises
{
    public class GetEnterprisesHandler : IRequestHandler<GetEnterprisesQuery, List<EnterpriseDto>>
    {
        private readonly IApplicationDbContext _context;

        // IMapper больше не нужен, его можно удалить из конструктора
        public GetEnterprisesHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EnterpriseDto>> Handle(GetEnterprisesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Enterprises
                .AsNoTracking()
                .Select(e => new EnterpriseDto
                {
                    // Вручную сопоставляем поля
                    Id = e.Id,
                    Name = e.Name,
                    LegalAddress = e.LegalAddress
                })
                .ToListAsync(cancellationToken);
        }
    }
}
