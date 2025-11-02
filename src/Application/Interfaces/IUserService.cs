using Application.Common;
using Application.Dtos;

namespace Application.Interfaces;

public interface IUserService
{
    Task<StandardResponse<UserDto>> GetUserByIdAsync(int id, CancellationToken ct);
    Task<PaginatedResponse<UserDto>> GetAllUsersAsync(PaginatedRequest pagination, CancellationToken ct);
    Task<StandardResponse> DeleteUserAsync(int id, string currentUserId, bool isAdmin, CancellationToken ct);
    Task<StandardResponse<UserDto>> UpdateUserAsync(string id, UpdateUserDto model, CancellationToken ct);

}