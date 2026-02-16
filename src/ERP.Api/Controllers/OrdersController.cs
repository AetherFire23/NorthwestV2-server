using Mediator;
using Microsoft.AspNetCore.Mvc;
using NorthwestV2.Practical;

namespace NortwestV2.Api.Controllers;

[ApiController]
[Route("products")]
public class OrdersController : ControllerBase
{
    private readonly ErpContext _ctx;
    private readonly ILogger<MyController> _logger;
    private readonly IMediator _mediator;

    public OrdersController(ErpContext ctx, ILogger<MyController> logger, IMediator mediator)
    {
        _ctx = ctx;
        _logger = logger;
        _mediator = mediator;
    }

    //[HttpPost("create-order")]
    //[ProducesResponseType<Guid>(StatusCodes.Status200OK)]
    //public async Task<ActionResult<Guid>> CreateCompany([FromBody] CreateOrderRequest createOrderRequest)
    //{
    //    _logger.LogInformation("Create company endpoint  reached.");
    //    Guid companyId = await _mediator.Send(createOrderRequest);
    //    return Ok(companyId);
    //}
}