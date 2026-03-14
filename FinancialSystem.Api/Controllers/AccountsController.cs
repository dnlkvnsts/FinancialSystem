using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Features.Accounts.Commands.CreateAccount;
using FinancialSystem.Application.Features.Accounts.Queries.GetAccountById;
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

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateAccountCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> Get(int id)
        {
            return await _mediator.Send(new GetAccountByIdQuery(id));
        }
    }
}
