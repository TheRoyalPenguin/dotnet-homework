using _3HW.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace _3HW.Controllers;

[ApiController]
[Route("api")]
public class MyAuthController(IUserService userService) : Controller
{
    [HttpPost("auth/login")]
    public IActionResult Login(string email, string password)
    {
        var result = userService.Login(email, password);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    [HttpPost("users/register")]
    public IActionResult Register(string email, string name, int age, string password)
    {
        var result = userService.Register(email, name, age, password);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);

    }
    [HttpPut("users/update")]
    public IActionResult UpdateUser(string email, string? name, int? age, string? password)
    {
        var result = userService.UpdateUser(email, name, age, password);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    [HttpDelete("users/delete")]
    public IActionResult DeleteUser(string email)
    {
        var result = userService.DeleteUser(email);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }

    [HttpGet("users/all")]
    public IActionResult GetAllUsers()
    {
        var result = userService.GetAllUsers();
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="createddate">Дата создания пользователя, формат DateTime, без часового пояса (не вводить +xx:xx в конце</param>
    /// <param name="updateddate">Дата обновления пользователя, формат DateTime, без часового пояса (не вводить +xx:xx в конце)</param>
    /// <returns>Возвращает json-строку с почтами пользователей, соответствующих фильтру</returns>
    [HttpGet("users/byTime")]
    public IActionResult GetUsersByTime(DateTime? createddate, DateTime? updateddate)
    {
        var result = userService.GetUsersByTime(createddate, updateddate);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
}
