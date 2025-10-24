using _7HW_Libraries.Data;
using _7HW_Libraries.Enums;
using _7HW_Libraries.Interfaces;
using _7HW_Libraries.Models;
using _7HW_Libraries.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace _7HW_Libraries.Services;
public class UserService(AppDbContext _context) : IUserService
{
    public async Task<Result> Login(string email, string password)
    {
        int passwordHash = password.GetHashCode();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == passwordHash);
        if (user == null)
            return Result.Error(401,"Неверный логин или пароль.");

        return Result.Success(200, "Успешно.");
    }
    public async Task<Result> Register(string email, string name, int age, Gender gender, string password)
    {
        int passwordHash = password.GetHashCode();

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
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

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        return Result.Success(201, "Успешно.");
    }
    public async Task<Result> UpdateUser(string email, string? name, int? age, string? password)
    {
        int passwordHash = password.GetHashCode();

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            return Result.Error(404,"Пользователь с таким email не найден.");

        if (user.Name != name && !string.IsNullOrWhiteSpace(name))
            user.Name = name;
        if (age.HasValue && user.Age != age && age > 0)
            user.Age = age.Value;
        if (user.PasswordHash != passwordHash)
            user.PasswordHash = passwordHash;
        user.UpdatedDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return Result.Success(200, "Успешно.");
    }
    public async Task<Result> DeleteUser(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            return Result.Error(404, "Пользователь с таким email не найден.");
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        
        return Result.Success(200, "Успешно.");
    }

    public async Task<Result> GetAllUsers()
    {
        var  users = await _context.Users.Select(u => u.Email).ToListAsync();
        return Result.Success(200, JsonConvert.SerializeObject(users));
    }
    public async Task<Result> GetUsersByPage(int pageNumber, int pageSize = 5)
    {
        if (pageSize <= 0)
            pageSize = 5;
        if (pageNumber <= 0)
            return Result.Error(400, "Номер страницы должен быть положительным.");
        var usersCount = await _context.Users.CountAsync();
        if (usersCount <= ((pageNumber - 1) * pageSize))
            pageNumber = usersCount / pageSize;

        var users = await _context.Users.Select(u => u.Email).Skip((pageNumber - 1) * pageSize).Take(pageSize)
            .ToListAsync();
        return Result.Success(200, JsonConvert.SerializeObject(users));
    }

    public async Task<Result> GetUsersByTime(DateTime? createddate, DateTime? updateddate)
    {
        if (createddate == null || updateddate == null)
        {
            if (createddate == null && updateddate == null)
                return await GetAllUsers();
            if (createddate == null)
                return Result.Success(200, JsonConvert.SerializeObject(_context.Users.Where(u => u.UpdatedDate == updateddate).Select(u => u.Email).ToListAsync()));
            if  (updateddate == null)
                return Result.Success(200,JsonConvert.SerializeObject(_context.Users.Where(u => u.CreatedDate == createddate).Select(u => u.Email).ToListAsync()));
        }

        return Result.Success(200, JsonConvert.SerializeObject(_context.Users.Where(u => u.CreatedDate == createddate && u.UpdatedDate == updateddate).Select(u => u.Email).ToListAsync()));
    }

    public async Task<Result> GetMinRegistrationDate()
    {
        return Result.Success(200, JsonConvert.SerializeObject(await _context.Users.MinAsync(u => u.CreatedDate)));
    }

    public async Task<Result> GetMaxRegistrationDate()
    {
        return Result.Success(200, JsonConvert.SerializeObject(await _context.Users.MaxAsync(u => u.CreatedDate)));
    }

    public async Task<Result> GetSortedUsersByName()
    {
        return Result.Success(200, JsonConvert.SerializeObject(await _context.Users.OrderBy(u => u.Name).Select(u => u.Name).ToListAsync()));
    }

    public async Task<Result> GetUsersByGender(Gender gender)
    {
        return Result.Success(200, JsonConvert.SerializeObject(await _context.Users.Where(u => u.Gender == gender).Select(u => u.Name).ToListAsync()));
    }

    public async Task<Result> GetUserCount()
    {
        var count = await _context.Users.CountAsync();
        return Result.Success(200, count.ToString());
    }
}
