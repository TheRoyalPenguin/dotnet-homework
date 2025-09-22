using _3HW.Utils;

namespace _3HW.Interfaces;

public interface IUserService
{
    public Result Login(string email, string password);
    public Result Register(string email, string name, int age, string password);
    public Result UpdateUser(string email, string? name, int? age, string? password);
    public Result DeleteUser(string email);
    public Result GetAllUsers();
    public Result GetUsersByTime(DateTime? createddate, DateTime? updateddate);
}