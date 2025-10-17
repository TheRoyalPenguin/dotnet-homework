using _6HW_EF.Models;
using Microsoft.EntityFrameworkCore;

namespace _6HW_EF.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {}
    
    public DbSet<User> Users { get; set; }
}