using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Features.Users.Commands.ConfirmClient;
using FinancialSystem.Application.Features.Users.Commands.RegisterClient;
using FinancialSystem.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<int>> Register([FromBody] RegisterClientCommand command)
        {
            // MediatR сам найдет нужный Handler и передаст ему команду
            var userId = await _mediator.Send(command);

            return Ok(userId);
        }


        [HttpPost("confirm")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> Confirm([FromBody] ConfirmClientCommand command)
        {
            // MediatR найдет твой ConfirmClientCommandHandler
            await _mediator.Send(command);

            // Возвращаем статус 200 OK, так как метод возвращает Unit (ничего)
            return Ok();
        }


        [HttpGet("unconfirmed")]
        [Authorize(Roles = "Manager")] // ТОЛЬКО для менеджеров
        public async Task<ActionResult<List<UnconfirmedUserDto>>> GetUnconfirmed()
        {
            var query = new GetUnconfirmedUsersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
