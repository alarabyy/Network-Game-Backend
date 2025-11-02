using Application.Common;
using Application.Interfaces;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Presentation.Endpoints.User;

public class Delete(IUserService service) : EndpointWithoutRequest<StandardResponse>
{
    public override void Configure()
    {
        Delete("/api/user/{id:int}");
        Description(b => b.WithTags("Users"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");

        // Extract user info from JWT
        var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                            ?? User.FindFirst("sub")?.Value
                            ?? string.Empty;

        var isAdmin = User.IsInRole("Admin");

        var response = await service.DeleteUserAsync(id, currentUserId, isAdmin, ct);
        await Send.OkAsync(response, ct);
    }
}