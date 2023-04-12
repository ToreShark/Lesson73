using AuthTest.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthTest.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

}