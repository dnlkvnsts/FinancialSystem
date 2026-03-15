using FinancialSystem.Application.DTOs;
using FinancialSystem.Application.Features.Enterprises.Queries;
using FinancialSystem.Application.Features.Enterprises.Queries.GetEnterprises;
using MediatR;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;

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

    }
}
