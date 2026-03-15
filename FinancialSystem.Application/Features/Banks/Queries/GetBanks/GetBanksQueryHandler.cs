using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Banks.Queries.GetBanks
{
    public class GetBanksQueryHandler : IRequestHandler<GetBanksQuery, List<BankDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetBanksQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BankDto>> Handle(GetBanksQuery request, CancellationToken cancellationToken)
        {
            return await _context.Banks
                .Select(bank => new BankDto
                {
                    Id = bank.Id, // Берется из AggregateRoot
                    Name = bank.Name,
                    Bik = bank.Bik
                })
                .ToListAsync(cancellationToken);
        }
    }
}
