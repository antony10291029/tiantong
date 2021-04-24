using Microsoft.EntityFrameworkCore;

namespace Midos.Domain.Test
{
  public class PostgresDomainOptions<T>: DomainContextOptions<T>
    where T: DomainContext
  {
    public override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseNpgsql("Host=localhost;Port=5432;Database=midos.test;Username=postgres;Password=password");
    }
  }

  public class User
  {
    public long Id { get; private set; }

    public string Name { get; private set; }

    public User(string name)
    {
      Name = name;
    }
  }

  public class TestDomainContext: DomainContext
  {
    public DbSet<User> Users { get; set; }

    public TestDomainContext(): base(
      new PostgresDomainOptions<TestDomainContext>(),
      new TestEventPublisher()
    ) {

    }
  }
}
