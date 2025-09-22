namespace _3HW.Models;

public class User
{
    public string Email { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public int PasswordHash { get; set; }
}
