using System.Text.Encodings.Web;
using System.Text.Json;
using _4HW_LINQ.Enums;
using _4HW_LINQ.Interfaces;
using _4HW_LINQ.Models;
using _4HW_LINQ.Utils;

namespace _4HW_LINQ.Services;
public class UserService : IUserService
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true
    };
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
    public Result Login(string email, string password)
    {
        int passwordHash = password.GetHashCode();
        var user = users.FirstOrDefault(u => u.Email == email && u.PasswordHash == passwordHash);
        if (user == null)
            return Result.Error(401,"Неверный логин или пароль.");

        return Result.Success(200, "Успешно.");
    }
    public Result Register(string email, string name, int age, Gender gender, string password)
    {
        int passwordHash = password.GetHashCode();

        var user = users.FirstOrDefault(u => u.Email == email);
        if (user != null)
            return Result.Error(409,"Пользователь с таким email уже существует.");
        if (age <= 0)
            return Result.Error(400,"Возраст должен быть положительным числом.");
        user = new User()
        {
            Email = email,
            Name = name,
            Age = age,
            Gender = gender,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now,
            PasswordHash = passwordHash
        };

        users.Add(user);
        return Result.Success(201, "Успешно.");
    }
    public Result UpdateUser(string email, string? name, int? age, string? password)
    {
        int passwordHash = password.GetHashCode();

        var user = users.FirstOrDefault(u => u.Email == email);
        if (user == null)
            return Result.Error(404,"Пользователь с таким email не найден.");

        if (user.Name != name && !string.IsNullOrWhiteSpace(name))
            user.Name = name;
        if (age.HasValue && user.Age != age && age > 0)
            user.Age = age.Value;
        if (user.PasswordHash != passwordHash)
            user.PasswordHash = passwordHash;
        user.UpdatedDate = DateTime.Now;

        return Result.Success(200, "Успешно.");
    }
    public Result DeleteUser(string email)
    {
        var user = users.FirstOrDefault(u => u.Email == email);
        if (user == null)
            return Result.Error(404, "Пользователь с таким email не найден.");
        
        users.Remove(user);

        return Result.Success(200, "Успешно.");
    }

    public Result GetAllUsers()
    {
        return Result.Success(200, JsonSerializer.Serialize(users.Select(u => u.Email), _options));
    }
    public Result GetUsersByPage(int pageNumber, int pageSize = 5)
    {
        if (pageSize <= 0)
            pageSize = 5;
        if (pageNumber <= 0)
            return Result.Error(400, "Номер страницы должен быть положительным.");
        if (users.Count <= ((pageNumber - 1) * pageSize))
            pageNumber = users.Count / pageSize;
        return Result.Success(200, JsonSerializer.Serialize(users.Select(u => u.Email).Skip((pageNumber - 1) * pageSize).Take(pageSize), _options));
    }

    public Result GetUsersByTime(DateTime? createddate, DateTime? updateddate)
    {
        if (createddate == null || updateddate == null)
        {
            if (createddate == null && updateddate == null)
                return GetAllUsers();
            if (createddate == null)
                return Result.Success(200, JsonSerializer.Serialize(users.Where(u => u.UpdatedDate == updateddate).Select(u => u.Email), _options));
            if  (updateddate == null)
                return Result.Success(200,JsonSerializer.Serialize(users.Where(u => u.CreatedDate == createddate).Select(u => u.Email), _options));
        }

        return Result.Success(200, JsonSerializer.Serialize(users.Where(u => u.CreatedDate == createddate && u.UpdatedDate == updateddate).Select(u => u.Email), _options));
    }

    public Result GetMinRegistrationDate()
    {
        return Result.Success(200, JsonSerializer.Serialize(users.Min(u => u.CreatedDate)));
    }

    public Result GetMaxRegistrationDate()
    {
        return Result.Success(200, JsonSerializer.Serialize(users.Max(u => u.CreatedDate)));
    }

    public Result GetSortedUsersByName()
    {
        return Result.Success(200, JsonSerializer.Serialize(users.OrderBy(u => u.Name).Select(u => u.Name), _options));
    }

    public Result GetUsersByGender(Gender gender)
    {
        return Result.Success(200, JsonSerializer.Serialize(users.Where(u => u.Gender == gender).Select(u => u.Name), _options));
    }

    public Result GetUserCount()
    {
        return Result.Success(200, users.Count().ToString());
    }
}
