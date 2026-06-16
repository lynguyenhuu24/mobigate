using MediatR;
using Microsoft.AspNetCore.Mvc;
using MobiGate.Application.Auth.Login;
using MobiGate.Application.Auth.Register;

namespace MobiGate.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IResult> Register(RegisterCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Results.Created($"/api/v1/auth/users/{result.Value!.UserId}", result.Value);
    }

    [HttpPost("login")]
    public async Task<IResult> Login(LoginCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return Results.Ok(result.Value);
    }
}
