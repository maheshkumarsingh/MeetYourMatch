namespace API.DTOs;

public class UserDTO
{
    public required string UserName { get; set; } = string.Empty;
    public required string KnownAs { get; set; } = string.Empty;
    public required string Token { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
}
