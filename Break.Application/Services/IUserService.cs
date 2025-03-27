using Break.Application.Models;

namespace Break.Application.Services;

public interface IUserService
{
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(User user, string password);
    Task<bool> ValidateUserCredentialsAsync(string username, string password);
    Task<string> GenerateJwtTokenAsync(User user);
}
