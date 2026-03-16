using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Common.Behaviors
{

    public class AuditLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : notnull
    {
        private readonly IApplicationDbContext _context;
        // Добавь сервис для получения текущего пользователя (если он у тебя есть)
        // private readonly ICurrentUserService _currentUserService; 

        public AuditLoggingBehavior(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            // 1. Выполняем саму команду (сначала действие, потом лог)
            var response = await next();

            var requestName = request.GetType().Name;

            // 2. Логируем только команды, ИСКЛЮЧАЯ те, что содержат пароли
            if (requestName.EndsWith("Command") &&
                !requestName.Contains("Login") &&
                !requestName.Contains("Register"))
            {
                var log = new AuditLog
                {
                    // В будущем замени 1 на _currentUserService.UserId
                    UserId = 1,
                    Action = requestName,
                    // Сериализуем данные запроса (теперь это безопасно)
                    Details = JsonSerializer.Serialize(request),
                    CreatedAt = DateTime.UtcNow
                };

                _context.AuditLogs.Add(log);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return response;
        }
    }
}
