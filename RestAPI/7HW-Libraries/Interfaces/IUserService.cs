using _7HW_Libraries.Enums;
using _7HW_Libraries.Utils;

namespace _7HW_Libraries.Interfaces;

public interface IUserService
{
    public Task<Result> Login(string email, string password);
    public Task<Result> Register(string email, string name, int age, Gender gender, string password);
    public Task<Result> UpdateUser(string email, string? name, int? age, string? password);
    public Task<Result> DeleteUser(string email);
    public Task<Result> GetAllUsers();
    public Task<Result> GetUsersByPage(int pageNumber, int pageSize);
    public Task<Result> GetUsersByTime(DateTime? createddate, DateTime? updateddate);
    public Task<Result> GetMinRegistrationDate();
    public Task<Result> GetMaxRegistrationDate();
    public Task<Result> GetSortedUsersByName();
    public Task<Result> GetUsersByGender(Gender gender);
    public Task<Result> GetUserCount();
}
