using Microsoft.AspNetCore.Mvc;

namespace RestAPI;

[ApiController]
[Route("api/auth")]
public class MyAuthController : Controller
{
    static List<User> users = [];
    [HttpPost("login")]
    public IActionResult Login(string email, string password)
    {
        int passwordHash = password.GetHashCode();
        var user = users.FirstOrDefault(u => u.Email == email && u.PasswordHash == passwordHash);
        if (user == null)
            return BadRequest("Неверный логин или пароль.");

        return Ok();
    }
    [HttpPost("create")]
    public IActionResult CreateUser(string email, string name, int age, string password)
    {
        int passwordHash = password.GetHashCode();

        var user = users.FirstOrDefault(u => u.Email == email);
        if (user != null)
            return BadRequest("Пользователь с таким email уже существует.");
        if (age <= 0)
            return BadRequest("Возраст должен быть положительным числом.");
        user = new User()
        {
            Email = email,
            Name = name,
            Age = age,
            PasswordHash = passwordHash
        };

        users.Add(user);
        return Created();
    }
    [HttpPut("update")]
    public IActionResult UpdateUser(string email, string? name, int? age, string? password)
    {
        int passwordHash = password.GetHashCode();

        var user = users.FirstOrDefault(u => u.Email == email);
        if (user == null)
            return NotFound("Пользователь с таким email не найден.");

        if (user.Name != name && !string.IsNullOrWhiteSpace(name))
            user.Name = name;
        if (age.HasValue && user.Age != age && age > 0)
            user.Age = age.Value;
        if (user.PasswordHash != passwordHash)
            user.PasswordHash = passwordHash;

        return Ok();
    }
    [HttpDelete("delete")]
    public IActionResult DeleteUser(string email)
    {
        var user = users.FirstOrDefault(u => u.Email == email);
        if (user == null)
            return NotFound("Пользователь с таким email не найден.");

        users.Remove(user);

        return Ok();
    }
}
