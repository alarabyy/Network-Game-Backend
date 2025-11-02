using Microsoft.AspNetCore.Identity;
using Application.Models.Writer;
using Application.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Entities;
using Application.Common;
using Application.Dtos;
using DataAccess.Types;

namespace Application.Services;

public class WriterService(
    UserManager<ApplicationUser> userManager,
    IWriterApplicationRequestRepository writerApplicationRequestRepository
    ) 
    : IWriterService
{
    public async Task<List<WriterApplicationRequestDto>> GetAllAsync(CancellationToken ct)
    {
        var requests = await writerApplicationRequestRepository.GetAllWithUserAsync(ct);
        return requests.Select(r => new WriterApplicationRequestDto(
            r.Id, 
            r.Type,
            r.ApplicationDate,
            r.ApprovedDate,
            new UserDto(r.UserId, null, r.User?.UserName)
        )).ToList();
    }

    public async Task<StandardResponse> ApplyAsync(int id, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return StandardResponse.Failed("User not found.");
        }
        
        var userRoles = await userManager.GetRolesAsync(user);
        if (userRoles.Contains("Writer"))
        {
            return StandardResponse.Failed("User already is a Writer.");
        }

        var userHaveUnprocessedRequests =
            await writerApplicationRequestRepository.UserHaveUnprocessedRequestAsync(id, ct);

        if (userHaveUnprocessedRequests)
        {
            return StandardResponse.Failed("User already have an unprocessed request.");
        }

        var request = new WriterApplicationRequest
        {
            UserId = user.Id
        };
        
        await writerApplicationRequestRepository.InsertAsync(request, ct);
        return StandardResponse.Succeeded("Applied successfully.");
    }

    public async Task<StandardResponse> AcceptAsync(WriterApplicationAcceptanceRequest request, CancellationToken ct)
    {
        var application = await writerApplicationRequestRepository.GetWithUserAsync(request.ApplicationId, ct);
        if (application == null)
        {
            return StandardResponse.Failed("Request not found.");
        }

        if (application.User == null)
        {
            return StandardResponse.Failed("User not found.");
        }

        if (application.Type != null)
        {
            return StandardResponse.Failed("Request already accepted.");
        }

        var isWriter = await userManager.IsInRoleAsync(application.User, "Writer");
        if (isWriter)
        {
            return StandardResponse.Failed("User already is a Writer.");
        }

        if (request.Accepted)
        {
            application.User.WriterType = request.Type;
            await userManager.RemoveFromRoleAsync(application.User, "Reader");
            await userManager.AddToRoleAsync(application.User, "Writer");
        }
        
        application.Type = request.Type;
        application.ApprovedDate = DateTime.UtcNow;
        
        await writerApplicationRequestRepository.UpdateAsync(application, ct);
        await userManager.UpdateAsync(application.User);
        
        return StandardResponse.Succeeded("Accepted successfully.");
    }

    public async Task<StandardResponse> RemoveAsync(int id, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return StandardResponse.Failed("User not found.");
        }
        
        var isWriter = await userManager.IsInRoleAsync(user, "Writer");
        if (!isWriter)
        {
            return StandardResponse.Failed("User is not a Writer.");
        }

        user.WriterType = null;
        
        await userManager.RemoveFromRoleAsync(user, "Writer");
        await userManager.AddToRoleAsync(user, "Reader");
        await userManager.UpdateAsync(user);
        
        return StandardResponse.Succeeded("Removed successfully.");
    }
}