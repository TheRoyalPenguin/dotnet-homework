using _7HW_Libraries.Enums;
using _7HW_Libraries.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace _7HW_Libraries.Controllers;

[ApiController]
[Route("api")]
public class MyAuthController(IUserService userService) : ControllerBase
{
    // Вход
    [HttpPost("auth/login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var result = await userService.Login(email, password);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    // Регистрация
    [HttpPost("users/register")]
    public async Task<IActionResult> Register(string email, string name, int age, Gender gender, string password)
    {
        var result = await userService.Register(email, name, age, gender, password);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);

    }
    // Обновление пользователя
    [HttpPut("users/update")]
    public async Task<IActionResult> UpdateUser(string email, string? name, int? age, string? password)
    {
        var result = await userService.UpdateUser(email, name, age, password);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    // Удаление пользователя по email
    [HttpDelete("users/delete")]
    public async Task<IActionResult> DeleteUser(string email)
    {
        var result = await userService.DeleteUser(email);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    // Получение json со всеми email пользователей
    [HttpGet("users/all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await userService.GetAllUsers();
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    // Получение json со всеми email пользователей по 'страницам' (5 на каждой странице)
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
    // Получение json с минимальной датой регистрации пользователей
    [HttpGet("users/min-registration-date")]
    public async Task<IActionResult> GetMinRegistrationDate()
    {
        var result = await userService.GetMinRegistrationDate();
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    // Получение json с максимальной датой регистрации пользователей
    [HttpGet("users/max-registration-date")]
    public async Task<IActionResult> GetMaxRegistrationDate()
    {
        var result = await userService.GetMaxRegistrationDate();
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    // Получение json со всеми именами пользователей в алфавитном порядке
    [HttpGet("users/sorted-by-name")]
    public async Task<IActionResult> GetSortedUsersByName()
    {
        var result = await userService.GetSortedUsersByName();
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    // Получение json со всеми email пользователей с полом 'gender'
    [HttpGet("users/filtered-by-gender")]
    public async Task<IActionResult> GetUsersByGender(Gender gender)
    {
        var result = await userService.GetUsersByGender(gender);
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
    // Получение строки с кол-вом всех зарегистрированных пользователей
    [HttpGet("users/count")]
    public async Task<IActionResult> GetUserCount()
    {
        var result = await userService.GetUserCount();
        return result.IsSuccess ? Ok(result.Message) : StatusCode(result.Status, result.Message);
    }
}
