namespace Midos.Domain
{
  public interface IUnitOfWork
  {
    int SaveChanges();
  }
}
