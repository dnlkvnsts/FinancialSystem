using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace FinancialSystem.Application.Features.Users.Commands.LoginClient
{
    public class LoginClientCommandHandler : IRequestHandler<LoginClientCommand, AuthResponseDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginClientCommandHandler(IApplicationDbContext context, IJwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponseDto> Handle(LoginClientCommand request, CancellationToken cancellationToken)
        {
            // 1. Ищем пользователя в базе
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            // 2. Проверяем, существует ли он и совпадает ли пароль
            if (user == null || user.PasswordHash != request.Password)
            {
                throw new Exception("Неверный email или пароль");
            }

            // --- НОВЫЙ БЛОК ПРОВЕРКИ ---
            // 3. Проверяем, подтвержден ли аккаунт (IsConfirmed)
            // Если в твоей сущности User это поле называется иначе (например, Status), подставь его
            if (!user.IsConfirmed)
            {
                // Если пользователь не подтвержден, выбрасываем ошибку и токен НЕ выдается
                throw new Exception("Ваш аккаунт еще не подтвержден менеджером.");
            }
            // ---------------------------

            // 4. Генерируем токен
            var token = _jwtTokenGenerator.GenerateToken(user);

            // 5. Возвращаем DTO
            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }
    }
}
