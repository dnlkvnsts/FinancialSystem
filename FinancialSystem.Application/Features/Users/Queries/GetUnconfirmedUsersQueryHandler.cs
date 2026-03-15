using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace FinancialSystem.Application.Features.Users.Queries
{
    public class GetUnconfirmedUsersQueryHandler : IRequestHandler<GetUnconfirmedUsersQuery, List<UnconfirmedUserDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetUnconfirmedUsersQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UnconfirmedUserDto>> Handle(GetUnconfirmedUsersQuery request, CancellationToken cancellationToken)
        {
            // Логика получения данных
            var unconfirmedUsers = await _context.Users
                .Where(u => !u.IsConfirmed)
                .Select(u => new UnconfirmedUserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Role = u.Role.ToString()
                })
                .ToListAsync(cancellationToken);

            return unconfirmedUsers;
        }
    }
}
