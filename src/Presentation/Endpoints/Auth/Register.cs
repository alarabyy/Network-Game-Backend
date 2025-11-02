using Application.Models.Authentication;
using Application.Interfaces;
using Application.Common;
using FastEndpoints;

namespace Presentation.Endpoints.Auth;

public class Register(IAuthenticationService service) : Endpoint<RegisterRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/api/auth/register");
        Description(r=> r.WithTags("Authentication"));
        AllowAnonymous();
    }
    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        var response = await service.RegisterAsync(req, ct); 
        await Send.OkAsync(response, ct);
    }
}