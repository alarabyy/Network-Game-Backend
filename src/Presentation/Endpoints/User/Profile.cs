using Presentation.Extensions;
using Application.Interfaces;
using Application.Common;
using Application.Dtos;
using FastEndpoints;

namespace Presentation.Endpoints.User;

public class Profile(IUserService service) : EndpointWithoutRequest<StandardResponse<UserDto>>
{
    public override void Configure()
    {
        Get("/api/user/profile");
        Description(r=> r.WithTags("Users"));
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = User.GetId();
        if (id == null)
        {
            await Send.UnauthorizedAsync(ct);
        }
        
        var response = await service.GetUserByIdAsync(id!.Value, ct);
        await Send.OkAsync(response, ct);
    }
}