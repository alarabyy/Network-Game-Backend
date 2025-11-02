using Application.Interfaces;
using Application.Common;
using Application.Dtos;
using FastEndpoints;

namespace Presentation.Endpoints.User;

public class List(IUserService service) : Endpoint<PaginatedRequest, PaginatedResponse<UserDto>>
{
    public override void Configure()
    {
        Get("/api/users");
        Description(x => x.WithTags("Users"));
        Roles("Admin");
    }
    
    public override async Task HandleAsync(PaginatedRequest req, CancellationToken ct)
    {
        var response = await service.GetAllUsersAsync(req, ct);
        await Send.OkAsync(response, ct);
    }
}