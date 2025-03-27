using Break.Application.Database;
using Break.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Break.Application.Repositories;

public class UserRepository(BreakAppDbContext dbContext) : IUserRepository
{
    public async Task<User> CreateUserAsync(User user, string password)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await dbContext.Users.FindAsync(id);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
}
