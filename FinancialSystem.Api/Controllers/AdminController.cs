using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Features.Admin.Commands.UndoAction;
using FinancialSystem.Application.Features.Admin.Queries.GetAuditLogs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")] // Доступ только для администраторов
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // МЕТОД ДЛЯ ПОЛУЧЕНИЯ ЛОГОВ
        [HttpGet("audit-logs")]
        public async Task<ActionResult<List<AuditLogDto>>> GetAuditLogs()
        {
            var logs = await _mediator.Send(new GetAuditLogsQuery());
            return Ok(logs);
        }



        [HttpPost("undo/{logId}")]
        public async Task<ActionResult> Undo(int logId)
        {
            // Вместо { LogId = logId } пишем просто (logId) в круглых скобках
            await _mediator.Send(new UndoActionCommand(logId));

            return Ok(new { message = "Успешно" });
        }
    }
}
