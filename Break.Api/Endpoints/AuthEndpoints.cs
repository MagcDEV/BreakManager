using Break.Api.Mapping;
using Break.Application.Services;
using Break.Contracts.Requests;

namespace Break.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var authGroup = app.MapGroup("")
            .WithTags("Auth");

        authGroup.MapPost(
                ApiEnpoints.Auth.Login,
                async (LoginRequest request, IUserService userService) =>
                {
                    var isValid = await userService.ValidateUserCredentialsAsync(
                        request.Username,
                        request.Password
                    );

                    if (!isValid)
                        return Results.Unauthorized();

                    var user = await userService.GetUserByUsernameAsync(request.Username);
                    var token = await userService.GenerateJwtTokenAsync(user!);

                    return Results.Ok(new { Token = token });
                }
            )
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("Login");

        authGroup.MapPost(
                ApiEnpoints.Auth.Register,
                async (RegisterUserRequest request, IUserService userService) =>
                {
                    var newUser = request.MapToUser();
                    var user = await userService.CreateUserAsync(newUser, request.Password);

                    return Results.Created($"/users/{user.UserId}", null);
                }
            )
            .AllowAnonymous()
            .Produces(StatusCodes.Status201Created)
            .WithName("Register");

        // Add additional auth-related endpoints here
    }
}
