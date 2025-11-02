using Application.Interfaces;
using Application.Common;
using Application.Dtos;
using FastEndpoints;

namespace Presentation.Endpoints.User;

public class GetById(IUserService service) : EndpointWithoutRequest<StandardResponse<UserDto>>
{
    public override void Configure()
    {
        Get("/api/user/{id:int}");
        Description(r=> r.WithTags("Users"));
        Roles("Admin");
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var response = await service.GetUserByIdAsync(id, ct);
        await Send.OkAsync(response, ct);
    }
}