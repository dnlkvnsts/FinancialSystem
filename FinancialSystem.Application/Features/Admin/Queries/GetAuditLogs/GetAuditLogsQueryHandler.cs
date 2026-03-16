using FinancialSystem.Application.Common.Interfaces;
using FinancialSystem.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Features.Admin.Queries.GetAuditLogs
{

    public class GetAuditLogsQueryHandler : IRequestHandler<GetAuditLogsQuery, List<AuditLogDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAuditLogsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuditLogDto>> Handle(GetAuditLogsQuery request, CancellationToken cancellationToken)
        {
            // Используем Join, чтобы вытащить Email пользователя по его UserId
            return await _context.AuditLogs
                .OrderByDescending(x => x.Id) // Сначала самые свежие логи
                .Join(_context.Users,
                    log => log.UserId,       // Ключ в таблице логов
                    user => user.Id,         // Ключ в таблице пользователей
                    (log, user) => new AuditLogDto
                    {
                        Id = log.Id,
                        UserId = log.UserId,
                        UserEmail = user.Email,    // ТЕПЕРЬ ТУТ БУДЕТ EMAIL, А НЕ NULL
                        Action = log.Action,
                        Details = log.Details,
                        CreatedAt = log.CreatedAt  // НЕ ЗАБУДЬ ДОБАВИТЬ ЭТО ПОЛЕ В ЭНТИТИ AuditLog
                    })
                .ToListAsync(cancellationToken);
        }
    }
}
