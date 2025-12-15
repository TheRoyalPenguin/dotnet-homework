using gRPC_Client.Enums;
using gRPC_Client.Utils;

namespace gRPC_Client.Interfaces;

public interface IUserService
{
    public Result Login(string email, string password);
    public Result Register(string email, string name, int age, Gender gender, string password);
    public Result UpdateUser(string email, string? name, int? age, string? password);
    public Result DeleteUser(string email);
    public Result GetAllUsers();
    public Result GetUsersByPage(int pageNumber, int pageSize);
    public Result GetUsersByTime(DateTime? createddate, DateTime? updateddate);
    public Result GetMinRegistrationDate();
    public Result GetMaxRegistrationDate();
    public Result GetSortedUsersByName();
    public Result GetUsersByGender(Gender gender);
    public Result GetUserCount();
}
