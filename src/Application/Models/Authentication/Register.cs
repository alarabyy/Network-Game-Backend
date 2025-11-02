namespace Application.Models.Authentication;

public record RegisterRequest(string Name, string Email, string Password);