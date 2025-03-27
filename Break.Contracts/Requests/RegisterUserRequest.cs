namespace Break.Contracts.Requests;

public class RegisterUserRequest
{
    public required string Username { get; set; } = string.Empty;

    public required string Email { get; set; } = string.Empty;

    public required string Password { get; set; } = string.Empty;

    public required List<string> Roles { get; set; } = new();
}
