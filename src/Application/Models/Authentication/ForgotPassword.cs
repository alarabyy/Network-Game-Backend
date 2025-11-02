namespace Application.Models.Authentication;

public record ForgotPasswordRequest(string Email);

public record ResetPasswordRequest(string Email, string Token, string Password);