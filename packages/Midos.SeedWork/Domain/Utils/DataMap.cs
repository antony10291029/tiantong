using System.Collections.Generic;

namespace Midos.SeedWork.Domain
{
  public class DataMap<TEntity> where TEntity: IEntity
  {
    public List<long> Keys { get; set; } = new();

    public Dictionary<long, TEntity> Values { get; set; } = new();

    public DataMap(TEntity[] entities)
    {
      foreach (var entity in entities) {
        Keys.Add(entity.Id);
        Values.Add(entity.Id, entity);
      }
    }
  }
}
