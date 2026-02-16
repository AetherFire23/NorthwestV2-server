using AetherFire23.ERP.Domain.Entity;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using NorthwestV2.Application.Features.ProductFeature.Commands.ProductCreation;
using NorthwestV2.Practical;

namespace NortwestV2.Api.Controllers;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly ErpContext _ctx;
    private readonly ILogger<MyController> _logger;
    private readonly IMediator _mediator;

    public ProductsController(ErpContext ctx, ILogger<MyController> logger, IMediator mediator)
    {
        _ctx = ctx;
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("create-product")]
    [ProducesResponseType<Guid>(StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateCompany([FromBody] CreateProductRequest createCompanyRequest)
    {
        _logger.LogInformation("Create company endpoint  reached.");
        var companyId = await _mediator.Send(createCompanyRequest);
        return Ok(companyId);
    }

    [HttpGet("list")]
    [ProducesResponseType<IEnumerable<Product>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        _logger.LogInformation($"List products endpoints reached {DateTime.Now}");
        var companyId = await _mediator.Send(new GetProductsRequest());
        return Ok(companyId);
    }
}