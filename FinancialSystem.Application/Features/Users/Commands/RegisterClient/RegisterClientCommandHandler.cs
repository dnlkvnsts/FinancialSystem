using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Domain.Aggregates;
using FinancialSystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinancialSystem.Application.Features.Users.Commands.RegisterClient
{
    public class RegisterClientCommandHandler : IRequestHandler<RegisterClientCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public RegisterClientCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(RegisterClientCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (emailExists)
            {
                throw new Exception("Пользователь с таким Email уже существует!");
            }

            var user = new User(
                request.FullName,
                request.Email,
                request.Password,
                RoleType.Client
            );

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
