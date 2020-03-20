using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Wms.Api
{
  public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
  {
    public virtual DbContext DbContext { get; set; }

    public virtual IUnitOfWork UnitOfWork { get => DbContext; }

    public DbSet<TEntity> Table { get => DbContext.Set<TEntity>(); }

    public Repository(DbContext db)
    {
      DbContext = db;
    }

    public virtual TEntity Add(TEntity entity)
    {
      return DbContext.Add(entity).Entity;
    }

    public void AddRange(params TEntity[] entities)
    {
      DbContext.AddRange(entities);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
      DbContext.AddRange(entities);
    }

    public virtual TEntity Update(TEntity entity)
    {
      return DbContext.Update(entity).Entity;
    }

    public virtual bool Remove(Entity entity)
    {
      DbContext.Remove(entity);

      return true;
    }

  }

  public class Repository<TEntity, TKey> : Repository<TEntity>, IRepository<TEntity, TKey> where TEntity : Entity
  {
    public Repository(DbContext DbContext) : base(DbContext)
    {

    }

    public virtual bool Remove(TKey id)
    {
      var entity = DbContext.Find<TEntity>(id);

      if (entity == null) {
        return false;
      }

      DbContext.Remove(entity);

      return true;
    }

    public virtual TEntity Get(TKey id)
    {
      return DbContext.Find<TEntity>(id);
    }

  }
}
