using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Features.Enterprises.Commands.CreatePayrollRequest;
using FinancialSystem.Application.Features.Enterprises.Commands.ReceiveSalary;
using FinancialSystem.Application.Features.Enterprises.Commands.UpdatePayrollRequestStatus;
using FinancialSystem.Application.Features.Enterprises.Queries;
using FinancialSystem.Application.Features.Enterprises.Queries.GetEnterprises;
using FinancialSystem.Application.Features.Enterprises.Queries.GetEnterprisesWithEmployee;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnterpriseController : ControllerBase
    {
        private readonly IMediator _mediator;

        // Внедряем MediatR через конструктор
        public EnterpriseController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<ActionResult<List<EnterpriseDto>>> GetEnterprises(CancellationToken cancellationToken)
        {
            var query = new GetEnterprisesQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }


        [Authorize(Roles = "Client")] 

        [HttpPost("{enterpriseId}/payroll-request")]
        public async Task<ActionResult<int>> Create(int enterpriseId)
        {
            // Получаем ID текущего авторизованного пользователя из JWT токена
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            int clientId = int.Parse(userIdClaim);

            // Отправляем команду в Application слой через MediatR
            var command = new CreatePayrollRequestCommand(clientId, enterpriseId);
            var requestId = await _mediator.Send(command);

            return Ok(requestId);
        }


        [Authorize(Roles = "Manager")]
         [HttpPut("request/{id}/status")]
        public async Task<ActionResult> UpdateStatus(int id, [FromBody] UpdatePayrollRequestStatusCommand command)
        {
            if (id != command.Id) return BadRequest();

            await _mediator.Send(command);
            return NoContent();
        }


        [Authorize(Roles = "Client")]
        [HttpPost("payroll-requests/{id}/receive-salary")] // {id} — это ID заявки
        public async Task<ActionResult> ReceiveSalary(int id, [FromBody] ReceiveSalaryDto dto)
        {
            // 1. Извлекаем ID клиента из токена
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            int clientId = int.Parse(userIdClaim);

            // 2. Отправляем команду (ТЕПЕРЬ С ПЯТЫМ АРГУМЕНТОМ — dto.Amount)
            // Порядок: RequestId, BankId, AccountNumber, ClientId, Amount
            var command = new ReceiveSalaryCommand(id, dto.BankId, dto.AccountNumber, clientId, dto.Amount);

            await _mediator.Send(command);

            return Ok($"Средства в размере {dto.Amount} успешно перечислены на счет {dto.AccountNumber}.");
        }


        [Authorize(Roles = "Manager")] 
        [HttpGet("with-employees")]
        public async Task<ActionResult<List<EnterpriseWithEmployeesDto>>> GetWithEmployees()
        {
            var query = new GetEnterprisesWithEmployeesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}
