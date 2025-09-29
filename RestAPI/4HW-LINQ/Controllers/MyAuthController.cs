using _4HW_LINQ.Enums;
using _4HW_LINQ.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace _4HW_LINQ.Controllers;

[ApiController]
[Route("api")]
public class MyAuthController(IUserService userService) : ControllerBase
{
    [HttpPost("auth/login")]
    public IActionResult Login(string email, string password)
    {
        var result = userService.Login(email, password);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    [HttpPost("users/register")]
    public IActionResult Register(string email, string name, int age, Gender gender, string password)
    {
        var result = userService.Register(email, name, age, gender, password);
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
    [HttpGet("users/page/{page}")]
    public IActionResult GetUsersByPage(int page)
    {
        if (page <= 0)
            return BadRequest("Номер страницы должен быть положительным.");
        var result = userService.GetUsersByPage(page, 5);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="createddate">Дата создания пользователя, формат DateTime, без часового пояса (не вводить +xx:xx в конце)</param>
    /// <param name="updateddate">Дата обновления пользователя, формат DateTime, без часового пояса (не вводить +xx:xx в конце)</param>
    /// <returns>Возвращает json-строку с почтами пользователей, соответствующих фильтру</returns>
    [HttpGet("users/byTime")]
    public IActionResult GetUsersByTime(DateTime? createddate, DateTime? updateddate)
    {
        var result = userService.GetUsersByTime(createddate, updateddate);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    [HttpGet("users/min-registration-date")]
    public IActionResult GetMinRegistrationDate()
    {
        var result = userService.GetMinRegistrationDate();
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    [HttpGet("users/max-registration-date")]
    public IActionResult GetMaxRegistrationDate()
    {
        var result = userService.GetMaxRegistrationDate();
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    [HttpGet("users/sorted-by-name")]
    public IActionResult GetSortedUsersByName()
    {
        var result = userService.GetSortedUsersByName();
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    [HttpGet("users/filtered-by-gender")]
    public IActionResult GetUsersByGender(Gender gender)
    {
        var result = userService.GetUsersByGender(gender);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    [HttpGet("users/count")]
    public IActionResult GetUserCount()
    {
        var result = userService.GetUserCount();
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
}
