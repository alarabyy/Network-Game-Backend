using Microsoft.AspNetCore.Identity;
using Application.Interfaces;
using DataAccess.Entities;
using Application.Common;
using Application.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public sealed class UserService(
    UserManager<ApplicationUser> userManager
    ) : IUserService
{
    public async Task<StandardResponse<UserDto>> UpdateUserAsync(string id, UpdateUserDto model, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null)
        {
            return StandardResponse<UserDto>.Failed("User not found.");
        }

        // ÊÍÏíË ÇáÞíã ÝÞØ ÅÐÇ ßÇäÊ ãæÌæÏÉ Ýí ÇáØáÈ
        if (!string.IsNullOrWhiteSpace(model.Name))
            user.UserName = model.Name;

        if (!string.IsNullOrWhiteSpace(model.Email))
            user.Email = model.Email;

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return StandardResponse<UserDto>.Failed($"Failed to update user. Errors: {errors}");
        }

        var updatedDto = new UserDto(user.Id, user.Email, user.UserName);
        return StandardResponse<UserDto>.Succeeded(updatedDto, "User updated successfully.");
    }

    public async Task<StandardResponse<UserDto>> GetUserByIdAsync(int id, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return StandardResponse<UserDto>.Failed("User not found.");
        }

        var dto = new UserDto(user.Id, user.Email, user.UserName);
        return StandardResponse<UserDto>.Succeeded(dto, "User found.");
    }

    public async Task<PaginatedResponse<UserDto>> GetAllUsersAsync(PaginatedRequest pagination, CancellationToken ct)
    {
        var query = userManager.Users;

        // Total count before pagination
        var totalCount = await query.CountAsync(ct);
        var pageCount = (int)Math.Ceiling(totalCount / (double)pagination.ItemsPerPage);

        // Ensure page is within range
        var currentPage = Math.Clamp(pagination.Page, 1, pageCount == 0 ? 1 : pageCount);

        var users = await query
            .OrderBy(u => u.Id)
            .Skip((currentPage - 1) * pagination.ItemsPerPage)
            .Take(pagination.ItemsPerPage)
            .Select(u => new UserDto(u.Id, u.Email, u.UserName))
            .ToListAsync(ct);

        return new PaginatedResponse<UserDto>(users, pageCount);
    }

    public async Task<StandardResponse> DeleteUserAsync(int id, string currentUserId, bool isAdmin, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return StandardResponse.Failed("User not found.");
        }

        // Allow only owner or admin
        if (!isAdmin && user.Id.ToString() != currentUserId)
        {
            return StandardResponse.Failed("You are not authorized to delete this account.");
        }

        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            return StandardResponse.Failed("Failed to delete user account.");
        }

        return StandardResponse.Succeeded($"User with ID {id} deleted successfully.");
    }

}