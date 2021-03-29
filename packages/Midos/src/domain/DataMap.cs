using System.Collections.Generic;
using System.Linq;

namespace Midos.Domain
{
  public interface IDataMap<TEntity, TKey> where TEntity: IEntity<TKey>
  {
    TKey[] Keys { get; }

    IDictionary<TKey, TEntity> Entities { get; }
  }

  public interface IDataMap<TEntity>: IDataMap<TEntity, long> where TEntity: IEntity<long>
  {

  }

  public class DataMap<TEntity, Tkey>: IDataMap<TEntity, Tkey> where TEntity: IEntity<Tkey>
  {
    public Tkey[] Keys { get; init; }

    public IDictionary<Tkey, TEntity> Entities { get; init; }

    public DataMap(TEntity[] entities) {
      Keys = entities.Select(entity => entity.Id).ToArray();
      Entities = entities.ToDictionary(entity => entity.Id, entity => entity);
    }
  }

  public class DataMap<TEntity>: DataMap<TEntity, long>, IDataMap<TEntity> where TEntity: IEntity<long>
  {
    public DataMap(TEntity[] entities) : base(entities) {}
  }
}
