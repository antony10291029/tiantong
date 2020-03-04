namespace Tiantong.Wms.Api
{
  public interface IRepository<TEntity>
  {
    IUnitOfWork UnitOfWork { get; }

    TEntity Add(TEntity entity);

    TEntity Update(TEntity entity);

    bool Remove(Entity entity);
  }

  public interface IRepository<TEntity, TKey> : IRepository<TEntity>
  {
    bool Remove(TKey id);

    TEntity Get(TKey id);

  }
}
