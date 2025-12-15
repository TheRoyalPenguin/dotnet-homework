using Gender = gRPC_UserService.Grpc.Gender;

namespace gRPC_UserService.Models;

public class User
{
    public string Email { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public int PasswordHash { get; set; }
}
