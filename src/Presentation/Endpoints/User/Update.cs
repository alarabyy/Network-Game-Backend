using Application.Interfaces;
using Application.Common;
using Application.Dtos;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Presentation.Endpoints.User
{
    public class UpdateUser(IUserService service) : Endpoint<UpdateUserDto, StandardResponse<UserDto>>
    {
        public override void Configure()
        {
            Put("/api/user/{id}");
            Description(b => b.WithTags("Users"));
            Roles("Admin");
        }

        public override async Task HandleAsync(UpdateUserDto req, CancellationToken ct)
        {
            var id = Route<string>("id"); // Use string for Identity user ID
            var response = await service.UpdateUserAsync(id, req, ct);

            // Simple response
            await Send.OkAsync(response, ct); // ✅ هذا هو الطريقة الصحيحة للـ FastEndpoints
        }
    }
}
