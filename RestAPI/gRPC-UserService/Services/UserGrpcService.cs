using gRPC_UserService.Grpc;
using gRPC_UserService.Models;
using Grpc.Core;

namespace gRPC_UserService.Services;

public class UserGrpcService : UserService.UserServiceBase
{
    static List<User> users = new()
    {
        new User { Email = "ivan@example.com",   Name = "Иван",   Age = 25, Gender = Gender.Male,   CreatedDate = DateTime.Parse("2023-01-10"), UpdatedDate = DateTime.Now, PasswordHash = 12345 },
        new User { Email = "masha@example.com",  Name = "Маша",   Age = 22, Gender = Gender.Female, CreatedDate = DateTime.Parse("2023-02-05"), UpdatedDate = DateTime.Now, PasswordHash = 54321 },
        new User { Email = "petr@example.com",   Name = "Пётр",   Age = 30, Gender = Gender.Male,   CreatedDate = DateTime.Parse("2023-03-15"), UpdatedDate = DateTime.Now, PasswordHash = 11111 },
        new User { Email = "olga@example.com",   Name = "Ольга",  Age = 27, Gender = Gender.Female, CreatedDate = DateTime.Parse("2023-04-01"), UpdatedDate = DateTime.Now, PasswordHash = 22222 },
        new User { Email = "sergey@example.com", Name = "Сергей", Age = 35, Gender = Gender.Male,   CreatedDate = DateTime.Parse("2023-05-20"), UpdatedDate = DateTime.Now, PasswordHash = 33333 },
        new User { Email = "anna@example.com",   Name = "Анна",   Age = 28, Gender = Gender.Female, CreatedDate = DateTime.Parse("2023-06-12"), UpdatedDate = DateTime.Now, PasswordHash = 44444 },
        new User { Email = "dima@example.com",   Name = "Дима",   Age = 19, Gender = Gender.Male,   CreatedDate = DateTime.Parse("2023-07-03"), UpdatedDate = DateTime.Now, PasswordHash = 55555 },
        new User { Email = "katya@example.com",  Name = "Катя",   Age = 24, Gender = Gender.Female, CreatedDate = DateTime.Parse("2023-08-18"), UpdatedDate = DateTime.Now, PasswordHash = 66666 },
        new User { Email = "nikita@example.com", Name = "Никита", Age = 32, Gender = Gender.Male,   CreatedDate = DateTime.Parse("2023-09-25"), UpdatedDate = DateTime.Now, PasswordHash = 77777 },
        new User { Email = "sveta@example.com",  Name = "Света",  Age = 21, Gender = Gender.Female, CreatedDate = DateTime.Parse("2023-10-30"), UpdatedDate = DateTime.Now, PasswordHash = 88888 }
    };
    public override Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
    {
        if (string.IsNullOrEmpty(request.Password))
        {
            return Task.FromResult(new RegisterResponse
                {
                    Success = false, 
                    Error = "Пароль не может быть пустым."
                });
        }
        
        var passwordHash = request.Password.GetHashCode();

        if (users.Any(u => u.Email == request.Email))
        {
            return Task.FromResult(new RegisterResponse
            {
                Success = false,
                Error = "Пользователь с таким email уже существует."
            });
        }
        
        if (request.Age <= 0)
        {
            return Task.FromResult(new RegisterResponse
            {
                Success = false,
                Error = "Возраст должен быть положительным числом."
            });
        }
        
        var user = new User()
        {
            Email = request.Email,
            Name = request.Name,
            Age = request.Age,
            Gender = request.Gender,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now,
            PasswordHash = passwordHash
        };
        users.Add(user);
        
        return Task.FromResult(new RegisterResponse
        {
            Success = true
        });
    }

    public override Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
        var passwordHash = request.Password.GetHashCode();
        
        var user = users.FirstOrDefault(u => 
            u.Email == request.Email && 
            u.PasswordHash == passwordHash);

        if (user == null)
        {
            return Task.FromResult(new LoginResponse
            {
                Success = false,
                Error = "Неверный логин или пароль."
            });
        }
        
        return Task.FromResult(new LoginResponse
        {
            Success = true
        });
    }
}