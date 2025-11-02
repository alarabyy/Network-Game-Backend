using Application.Models.Authentication;
using Application.Interfaces;
using Application.Common;
using FastEndpoints;

namespace Presentation.Endpoints.Auth;

public class Login(IAuthenticationService service) : Endpoint<LoginRequest, StandardResponse<LoginResponse>>
{
    public override void Configure()
    {
        Post("/api/auth/login");
        Description(r=> r.WithTags("Authentication"));
        AllowAnonymous();
    }
    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var response = await service.LoginAsync(req, ct); 
        await Send.OkAsync(response, ct);
    }
}