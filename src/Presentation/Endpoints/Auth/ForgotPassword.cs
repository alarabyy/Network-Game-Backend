using Application.Models.Authentication;
using Application.Interfaces;
using Application.Common;
using FastEndpoints;

namespace Presentation.Endpoints.Auth;

public class ForgotPassword(IAuthenticationService service) : Endpoint<ForgotPasswordRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/api/auth/forgot-password");
        Description(r=> r.WithTags("Authentication"));
        AllowAnonymous();
    }
    public override async Task HandleAsync(ForgotPasswordRequest req, CancellationToken ct)
    {
        var response = await service.ForgotPasswordAsync(req, ct); 
        await Send.OkAsync(response, ct);
    }
}