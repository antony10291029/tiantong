using Microsoft.EntityFrameworkCore;

namespace Midos.Domain
{
  public interface IDomainContextOptions
  {
    void OnConfiguring(DbContextOptionsBuilder builder);
  }

  public interface IDomainContextOptions<T>: IDomainContextOptions where T: DomainContext
  {

  }

  public abstract class DomainContextOptions: IDomainContextOptions
  {
    public abstract void OnConfiguring(DbContextOptionsBuilder builder);
  }

  public abstract class DomainContextOptions<T>: DomainContextOptions, IDomainContextOptions<T> where T: DomainContext
  {

  }
}
