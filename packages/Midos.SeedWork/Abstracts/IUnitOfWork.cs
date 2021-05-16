namespace Midos.SeedWork
{
  public interface IUnitOfWork
  {
    void SaveChanges();

    void BeginTransaction();

    void Commit();

    void Rollback();

    void Publish<T>(string msg, T data) where T: DomainEvent;
  }
}
