using Mediator;
using Microsoft.AspNetCore.Mvc;
using NorthwestV2.Application.Features.CompanyFeature.Commands.CreateCompany;
using NorthwestV2.Practical;

namespace NortwestV2.Api.Controllers;

[ApiController]
[Route("company")]
public class MyController : ControllerBase
{
    private readonly ErpContext _ctx;
    private readonly ILogger<MyController> _logger;
    private readonly IMediator _mediator;

    public MyController(ErpContext ctx, ILogger<MyController> logger, IMediator mediator)
    {
        _ctx = ctx;
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("createCompany")]
    [ProducesResponseType<Guid>(StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateCompany([FromBody] CreateCompanyRequest createCompanyRequest)
    {
        await Task.Delay(1000);
        _logger.LogInformation("Create company endpoint  reached.");
        var companyId = await _mediator.Send(createCompanyRequest);
        return Ok(companyId);
    }
}