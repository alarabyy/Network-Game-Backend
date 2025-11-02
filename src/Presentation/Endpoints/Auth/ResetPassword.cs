using Application.Models.Authentication;
using Application.Interfaces;
using Application.Common;
using FastEndpoints;

namespace Presentation.Endpoints.Auth;

public class ResetPassword(IAuthenticationService service) : Endpoint<ResetPasswordRequest, StandardResponse>
{
    public override void Configure()
    {
        Post("/api/auth/reset-password");
        Description(r=> r.WithTags("Authentication"));
        AllowAnonymous();
    }
    public override async Task HandleAsync(ResetPasswordRequest req, CancellationToken ct)
    {
        var response = await service.ResetPasswordAsync(req, ct); 
        await Send.OkAsync(response, ct);
    }
}