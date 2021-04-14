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
}
