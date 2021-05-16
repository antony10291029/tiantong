using Microsoft.EntityFrameworkCore;

namespace Midos.SeedWork
{
  public class EFContextOptions<T> where T: EFContext
  {
    public virtual void OnConfiguring(DbContextOptionsBuilder builder)
    {

    }

    public virtual void OnModelCreating(ModelBuilder builder)
    {

    }
  }
}
