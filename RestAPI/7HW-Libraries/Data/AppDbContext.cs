using _7HW_Libraries.Models;
using Microsoft.EntityFrameworkCore;

namespace _7HW_Libraries.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {}
    
    public DbSet<User> Users { get; set; }
}