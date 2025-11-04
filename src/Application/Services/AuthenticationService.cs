using Microsoft.Extensions.Configuration;
using Application.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Application.Interfaces;
using FastEndpoints.Security;
using DataAccess.Entities;
using Application.Common;

namespace Application.Services;

public sealed class AuthenticationService(
    UserManager<ApplicationUser> userManager,
    IEmailSenderService emailSender,
    IConfiguration configuration
    ) : IAuthenticationService
{
    public async Task<StandardResponse<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return StandardResponse<LoginResponse>.Failed("Email or Password is incorrect.");

        var isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
            return StandardResponse<LoginResponse>.Failed("Email or Password is incorrect.");

        var userRoles = await userManager.GetRolesAsync(user);
        
        string token;
        try
        {
            token = JwtBearer.CreateToken(o =>
            {
                o.SigningKey = configuration["JwtSettings:Key"]!;
                o.ExpireAt = DateTime.UtcNow.AddDays(1);
                o.User.Roles.Add(userRoles.FirstOrDefault() ?? "");
                o.User.Claims.Add((ClaimTypes.Role, userRoles.FirstOrDefault() ?? ""));
                o.User.Claims.Add((ClaimTypes.NameIdentifier, user.Id.ToString()));
                o.User.Claims.Add((ClaimTypes.Name, user.UserName!));
                o.User.Claims.Add((ClaimTypes.Email, user.Email!));
            });
        }
        catch (Exception ex)
        {
            return StandardResponse<LoginResponse>.Failed("Token generation failed: " + ex.Message);
        }

        var response = new LoginResponse(token);
        return StandardResponse<LoginResponse>.Succeeded(response, "Login successful.");
    }
    
    public async Task<StandardResponse> RegisterAsync(RegisterRequest request, CancellationToken ct)
    {
        var userByEmail = await userManager.FindByEmailAsync(request.Email);
        if (userByEmail != null)
        {
            return StandardResponse.Failed("Email already exists.");
        }
        
        var user = new ApplicationUser
        {
            UserName = request.Name,
            Email = request.Email,
            EmailConfirmed = true
        };
        
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return StandardResponse.Failed("Failed to register.");
        }
        
        await userManager.AddToRoleAsync(user, "Reader");
        
        return StandardResponse.Succeeded("Registration successful.");
    }

    public async Task<StandardResponse> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct)
    {
        var response = new StandardResponse(true,
            "If an account with this email exists, a password reset link has been sent.");
        
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return response;    
        }
        
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        await emailSender.SendEmailAsync(user.Email!, "Reset Your Password", $"Please reset your password by clicking here: {token}", ct);

        return response;
    }

    public async Task<StandardResponse> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            // Again, do not reveal user existence
            return  StandardResponse.Failed("Password reset failed. Please try again.");
        }

        var result = await userManager.ResetPasswordAsync(user, request.Token, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return StandardResponse.Failed($"Password reset failed: {errors}");
        }
        
        return StandardResponse.Succeeded("Your password has been reset successfully.");
        
    }
}