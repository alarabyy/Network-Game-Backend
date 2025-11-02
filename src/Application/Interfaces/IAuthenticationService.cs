using Application.Common;
using Application.Models.Authentication;

namespace Application.Interfaces;

public interface IAuthenticationService
{
    Task<StandardResponse<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct);
    Task<StandardResponse> RegisterAsync(RegisterRequest request, CancellationToken ct);
    Task<StandardResponse> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct);
    Task<StandardResponse> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct);
}