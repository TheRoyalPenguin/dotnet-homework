using System.Text.Json;
using _3HW.Interfaces;
using _3HW.Models;
using _3HW.Utils;

namespace _3HW.Services;
public class UserService: IUserService
{
    static List<User> users = [];
    public Result Login(string email, string password)
    {
        int passwordHash = password.GetHashCode();
        var user = users.FirstOrDefault(u => u.Email == email && u.PasswordHash == passwordHash);
        if (user == null)
            return Result.Error(401,"Неверный логин или пароль.");

        return Result.Success(200, "Успешно.");
    }
    public Result Register(string email, string name, int age, string password)
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
        return Result.Success(200, JsonSerializer.Serialize(users.Select(u => u.Email)));
    }

    public Result GetUsersByTime(DateTime? createddate, DateTime? updateddate)
    {
        if (createddate == null || updateddate == null)
        {
            if (createddate == null && updateddate == null)
                return GetAllUsers();
            if (createddate == null)
                return Result.Success(200, JsonSerializer.Serialize(users.Where(u => u.UpdatedDate == updateddate).Select(u => u.Email)));
            if  (updateddate == null)
                return Result.Success(200,JsonSerializer.Serialize(users.Where(u => u.CreatedDate == createddate).Select(u => u.Email)));
        }

        return Result.Success(200, JsonSerializer.Serialize(users.Where(u => u.CreatedDate == createddate && u.UpdatedDate == updateddate).Select(u => u.Email)));
    }
}