using Break.Application.Models;

namespace Break.Application.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(User user, string password);
}
