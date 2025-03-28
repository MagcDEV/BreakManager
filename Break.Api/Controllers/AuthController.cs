using Break.Api.Mapping;
using Break.Application.Services;
using Break.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Break.Api.Controllers;

[ApiController]
public class AuthController(IUserService userService) : ControllerBase
{
    [HttpPost(ApiEnpoints.Auth.Login)]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var isValid = await userService.ValidateUserCredentialsAsync(
            request.Username,
            request.Password
        );

        if (!isValid)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        var user = await userService.GetUserByUsernameAsync(request.Username);
        var token = await userService.GenerateJwtTokenAsync(user!);

        return Ok(new { Token = token });
    }

    [HttpPost(ApiEnpoints.Auth.Register)]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var newUser = request.MapToUser();

        var user = await userService.CreateUserAsync(newUser, request.Password);

        return CreatedAtAction(nameof(Register), new { id = user.UserId });
    }
}
