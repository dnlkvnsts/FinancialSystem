using FinancialSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace FinancialSystem.Application.Features.Users.Commands.ConfirmClient
{
    public class ConfirmClientCommandHandler : IRequestHandler<ConfirmClientCommand>
    {
        private readonly IApplicationDbContext _context;

        public ConfirmClientCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ConfirmClientCommand request, CancellationToken cancellationToken)
        {
            // 1. Ищем пользователя по ID
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
            {
                throw new Exception("Пользователь не найден."); // Позже мы сделаем красивые кастомные исключения
            }

            // 2. Вызываем доменную логику! 
            // Метод сам поменяет статус и добавит UserConfirmedEvent в список событий
            user.Confirm();

            // 3. Сохраняем в базу
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
