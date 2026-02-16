using Mediator;
using Microsoft.AspNetCore.Mvc;
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
}