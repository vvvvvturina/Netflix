using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess;

public class UserContextFactory: IDesignTimeDbContextFactory<UserContext>
{
    public UserContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
        optionsBuilder.UseSqlServer("server=localhost; database=Neflix; Trusted_Connection=false; TrustServerCertificate=true; Integrated Security=SSPI");

        return new UserContext(optionsBuilder.Options);
    }
}