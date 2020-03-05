using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Wms.Api
{
  public interface IRepository<TEntity> where TEntity : Entity
  {
    IUnitOfWork UnitOfWork { get; }

    DbSet<TEntity> Table { get; }

    TEntity Add(TEntity entity);

    TEntity Update(TEntity entity);

    bool Remove(Entity entity);
  }

  public interface IRepository<TEntity, TKey> : IRepository<TEntity> where TEntity : Entity
  {
    bool Remove(TKey id);

    TEntity Get(TKey id);

  }
}
