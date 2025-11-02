namespace Application.Models.Authentication;

public record LoginRequest(string Email, string Password, bool RememberMe);
public record LoginResponse(string Token);