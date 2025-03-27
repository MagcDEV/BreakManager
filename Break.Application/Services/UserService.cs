using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Break.Application.Database;
using Break.Application.Models;
using Break.Application.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Break.Application.Services;

public class UserService(
    IConfiguration configuration,
    IUserRepository userRepository,
    BreakAppDbContext dbContext
) : IUserService
{
    public async Task<User> CreateUserAsync(User user, string password)
    {
        user.PasswordHash = HashPassword(password);
        await userRepository.CreateUserAsync(user, password);
        return user;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await userRepository.GetUserByIdAsync(id);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await userRepository.GetUserByUsernameAsync(username);
    }

    public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
    {
        var user = await userRepository.GetUserByUsernameAsync(username);
        return user != null && VerifyPassword(password, user.PasswordHash);
    }

    public async Task<string> GenerateJwtTokenAsync(User user)
    {
        // Update last login
        user.LastLogin = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(
            configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT key not configured")
        );

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, user.Username)
        };

        // Add role claims
        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1), // Token expires after 1 hour
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    // Password hashing using BCrypt
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, 12); // 12 is the work factor
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
