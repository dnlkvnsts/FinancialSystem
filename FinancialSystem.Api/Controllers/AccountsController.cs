using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Features.Accounts.Commands.AccrueInterest;
using FinancialSystem.Application.Features.Accounts.Commands.CloseAccount;
using FinancialSystem.Application.Features.Accounts.Commands.OpenAccount;
using FinancialSystem.Application.Features.Accounts.Commands.TransferFunds;
using FinancialSystem.Application.Features.Accounts.Queries.GetAccountById;
using FinancialSystem.Application.Features.Accounts.Queries.GetAccountHistory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinancialSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator) => _mediator = mediator;


        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> Get(int id)
        {
            return await _mediator.Send(new GetAccountByIdQuery(id));
        }

        [HttpPost("open")]
        public async Task<ActionResult<int>> OpenAccount([FromBody] OpenAccountCommand command)
        {
            var accountId = await _mediator.Send(command);
            return Ok(accountId);
        }

        [HttpDelete("{id}")] // Обычно закрытие/удаление делают через DELETE
        public async Task<ActionResult> CloseAccount(int id)
        {
            await _mediator.Send(new CloseAccountCommand(id));
            return NoContent(); // Возвращаем 204 (успешно, без контента)
        }

        [HttpPost("transfer")]
        public async Task<ActionResult<TransferResponseDto>> Transfer([FromBody] TransferFundsCommand command)
        {
           
                // Отправляем команду в MediatR, он сам найдет нужный Handler
                var result = await _mediator.Send(command);

                // Возвращаем результат клиенту
                return Ok(result);
           
        }

        [HttpGet("{id}/history")]
        public async Task<ActionResult<List<TransactionDto>>> GetHistory(int id)
        {
            var history = await _mediator.Send(new GetAccountHistoryQuery(id));
            return Ok(history);
        }

        [HttpPost("{id}/accrue-interest")]
        public async Task<IActionResult> AccrueInterest(int id)
        {
            // Мы передаем ID из URL в нашу команду
            await _mediator.Send(new AccrueInterestCommand(id));

            return Ok(new { message = "Проценты успешно начислены на вклад" });
        }


    }
}
