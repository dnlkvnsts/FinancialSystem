using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Features.Users.Commands.LoginClient;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Путь будет: api/auth
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        // Внедряем MediatR через конструктор
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")] // Полный путь: POST api/auth/login
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginClientCommand command)
        {
            try
            {
                // Отправляем команду "почтальону" MediatR. 
                // Он сам найдет твой LoginClientCommandHandler и выполнит его!
                var result = await _mediator.Send(command);

                // Возвращаем статус 200 OK и наш токен с данными
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Если пароль не подошел, хендлер выбросит ошибку, и мы вернем 400 Bad Request
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
