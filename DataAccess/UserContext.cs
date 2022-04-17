using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public UserContext(DbContextOptions<UserContext> options)
        : base(options)
    {
    }

}