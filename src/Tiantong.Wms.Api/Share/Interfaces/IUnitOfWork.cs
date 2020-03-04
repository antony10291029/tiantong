namespace Tiantong.Wms.Api
{
  public interface IUnitOfWork
  {
    int SaveChanges();

    void BeginTransaction();

    bool HasTransaction();

    void Commit();

    void Rollback();
  }
}
