namespace Application.Dtos;

public record UserDto(int Id, string? Email, string? Name);
public class UpdateUserDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
}