using Microsoft.EntityFrameworkCore;
using Midos.App;
using Midos.Domain;

namespace Midos.Account.Api
{
  public class AppContext: DomainDbContext, IAppDbContext
  {
    protected DbSet<User> Users { get; set; }

    protected DbSet<Role> Roles { get; set; }

    protected DbSet<Policy> Policies { get; set; }

    protected DbSet<UserRole> UserRoles { get; set; }

    protected DbSet<RolePolicy> RolePolicies { get; set; }

    public AppContext(AppContextOptions options): base(options)
    {

    }
  }

  public class AppContextOptions: DomainDbContextOptions
  {
    private readonly AppConfig _config;

    public AppContextOptions(AppConfig config)
    {
      _config = config;
    }

    public override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseNpgsql(_config.AppDatabase);
    }

    public override void OnModelCreating(ModelBuilder builder)
    {

    }
  }
}
