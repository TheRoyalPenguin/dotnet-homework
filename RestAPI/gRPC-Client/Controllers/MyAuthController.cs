using gRPC_Client.Grpc;
using Microsoft.AspNetCore.Mvc;
using Gender = gRPC_Client.Enums.Gender;

namespace gRPC_Client.Controllers;

[ApiController]
[Route("api")]
public class MyAuthController(UserService.UserServiceClient grpc) : ControllerBase
{
    [HttpPost("auth/login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var result = await grpc.LoginAsync(new LoginRequest
        {
            Email = email,
            Password = password
        });
        
        return result.Success ? Ok(result.Jwt) : StatusCode(401, result.Error);
    }

    [HttpPost("users/register")]
    public async Task<IActionResult> Register(string email, string name, int age, Gender gender, string password)
    {
        var result = await grpc.RegisterAsync(new RegisterRequest
        {
            Email = email, 
            Name = name, 
            Age = age, 
            Gender = (Grpc.Gender)gender, 
            Password = password
        });
        return result.Success ? Ok(result.UserId) : StatusCode(500, result.Error);
    }
}
