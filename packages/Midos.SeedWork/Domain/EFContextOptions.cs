using Microsoft.EntityFrameworkCore;

namespace Midos.SeedWork.Domain
{
  public class EFContextOptions
  {
    public virtual void OnConfiguring(DbContextOptionsBuilder builder)
    {

    }

    public virtual void OnModelCreating(ModelBuilder builder)
    {

    }
  }

  public class EFContextOptions<T>: EFContextOptions where T: EFContext
  {

  }
}
