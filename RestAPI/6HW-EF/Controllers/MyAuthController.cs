using _6HW_EF.Enums;
using _6HW_EF.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace _6HW_EF.Controllers;

[ApiController]
[Route("api")]
public class MyAuthController(IUserService userService) : ControllerBase
{
    [HttpPost("auth/login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var result = await userService.Login(email, password);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    [HttpPost("users/register")]
    public async Task<IActionResult> Register(string email, string name, int age, Gender gender, string password)
    {
        var result = await userService.Register(email, name, age, gender, password);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);

    }
    [HttpPut("users/update")]
    public async Task<IActionResult> UpdateUser(string email, string? name, int? age, string? password)
    {
        var result = await userService.UpdateUser(email, name, age, password);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    [HttpDelete("users/delete")]
    public async Task<IActionResult> DeleteUser(string email)
    {
        var result = await userService.DeleteUser(email);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    [HttpGet("users/all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await userService.GetAllUsers();
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    [HttpGet("users/page/{page}")]
    public async Task<IActionResult> GetUsersByPage(int page)
    {
        if (page <= 0)
            return BadRequest("Номер страницы должен быть положительным.");
        var result = await userService.GetUsersByPage(page, 5);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="createddate">Дата создания пользователя, формат DateTime, без часового пояса (не вводить +xx:xx в конце)</param>
    /// <param name="updateddate">Дата обновления пользователя, формат DateTime, без часового пояса (не вводить +xx:xx в конце)</param>
    /// <returns>Возвращает json-строку с почтами пользователей, соответствующих фильтру</returns>
    [HttpGet("users/byTime")]
    public async Task<IActionResult> GetUsersByTime(DateTime? createddate, DateTime? updateddate)
    {
        var result = await userService.GetUsersByTime(createddate, updateddate);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    [HttpGet("users/min-registration-date")]
    public async Task<IActionResult> GetMinRegistrationDate()
    {
        var result = await userService.GetMinRegistrationDate();
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    [HttpGet("users/max-registration-date")]
    public async Task<IActionResult> GetMaxRegistrationDate()
    {
        var result = await userService.GetMaxRegistrationDate();
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    [HttpGet("users/sorted-by-name")]
    public async Task<IActionResult> GetSortedUsersByName()
    {
        var result = await userService.GetSortedUsersByName();
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    [HttpGet("users/filtered-by-gender")]
    public async Task<IActionResult> GetUsersByGender(Gender gender)
    {
        var result = await userService.GetUsersByGender(gender);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    [HttpGet("users/count")]
    public async Task<IActionResult> GetUserCount()
    {
        var result = await userService.GetUserCount();
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
}
