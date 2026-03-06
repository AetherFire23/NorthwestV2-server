using AetherFire23.ERP.Domain.Entity;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using NorthwestV2.Application.UseCases.Authentication.Login;
using NorthwestV2.Application.UseCases.Authentication.Register;
using NorthwestV2.Practical;

namespace NortwestV2.Api.Controllers;

[ApiController]
[Route("auth")]
public class MyController : ControllerBase
{
    private readonly NorthwestContext _ctx;
    private readonly ILogger<MyController> _logger;
    private readonly IMediator _mediator;

    public MyController(NorthwestContext ctx, ILogger<MyController> logger, IMediator mediator)
    {
        _ctx = ctx;
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<Guid>> Register(RegisterRequest request,
        CancellationToken cancellationToken)
    {
        Guid userId = await _mediator.Send(request, cancellationToken);
        return Ok(userId);
    }

    [HttpPut("login")]
    public async Task<ActionResult<Guid>> Login(LoginRequest request,
        CancellationToken cancellationToken)
    {
        LoginResult lg = await _mediator.Send(request, cancellationToken);
        return Ok(lg);
    }
    [HttpPut("test")]
    public async Task<ActionResult> Test(LoginRequest request,
        CancellationToken cancellationToken)
    {
        
        return Ok();
    }
}

