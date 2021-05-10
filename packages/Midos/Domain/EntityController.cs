using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Midos.Domain
{
  public abstract class EntityController<TEntity, TKey>
    where TEntity: class, IEntity<TKey>
  {
    private readonly DomainContext _context;

    private readonly ILogger<TEntity> _logger;

    public EntityController(DomainContext context, ILogger<TEntity> logger)
    {
      _context = context;
      _logger = logger;
    }

    [HttpPost("add")]
    public INotifyResult<IMessageObject> Add([FromBody] TEntity entity)
    {
      _context.Add(entity);
      _context.SaveChanges();
      _logger.LogInformation("数据已添加", entity);

      return NotifyResult.FromVoid().Success("数据添加成功");
    }

    public class RemoveParams
    {
      public TKey Id { get; set; }
    }

    [HttpPost("remove")]
    public INotifyResult<IMessageObject> Delete([FromBody] RemoveParams param)
    {
      var entity = _context.Find<TEntity>(param.Id);

      _context.Remove(entity);
      _context.SaveChanges();
      _logger.LogInformation("数据已删除", entity);

      return NotifyResult.FromVoid().Success("数据已删除");
    }

    [HttpPost("update")]
    public INotifyResult<IMessageObject> Update([FromBody] TEntity param)
    {
      var entity = _context.Find<TEntity>(param.Id);

      _context.Entry(entity).CurrentValues.SetValues(param);
      _context.SaveChanges();
      _logger.LogInformation("数据已更新", entity);

      return NotifyResult.FromVoid().Success("数据已更新");
    }

    public class FindParams
    {
      public TKey Id { get; set; }
    }

    [HttpPost("find")]
    public TEntity Find([FromBody] FindParams param)
    {
      return _context.Find<TEntity>(param.Id);
    }

    [HttpPost("search")]
    public IPagination<TEntity, TKey> Search([FromBody] QueryParams param)
    {
      var query = _context.Set<TEntity>().AsQueryable();

      query = query.OrderByDescending(entity => entity.Id);

      return query.Paginate<TEntity, TKey>(param);
    }

    [HttpPost("all")]
    public IDataMap<TEntity, TKey> All()
    {
      var query = _context.Set<TEntity>().AsQueryable();

      query = query.OrderByDescending(entity => entity.Id);

      return query.ToDataMap<TEntity, TKey>();
    }

    [HttpPost("batch/add")]
    public INotifyResult<IMessageObject> BatchAdd([FromBody] TEntity[] param)
    {
      var msg = "数据批量已创建";

      _context.AddRange(param);
      _context.SaveChanges();
      _logger.LogInformation(msg);      

      return NotifyResult.FromVoid().Success(msg);
    }

    [HttpPost("batch/update")]
    public INotifyResult<IMessageObject> BatchUpdate([FromBody] TEntity[] param)
    {
      var msg = "数据批量已更新";
      var dataMap = param.AsQueryable().ToDataMap<TEntity, TKey>();
      var ids = dataMap.Keys;
      var entities = _context.Set<TEntity>()
        .Where(entity => ids.Contains(entity.Id))
        .ToArray();

      foreach (var entity in entities) {
        _context.Entry(entity)
          .CurrentValues
          .SetValues(dataMap.Entities[entity.Id]);
      }

      _context.SaveChanges();
      _logger.LogInformation(msg);

      return NotifyResult.FromVoid().Success(msg);
    }

    public class BatchRemoveParams
    {
      public TKey[] Ids { get; set; }
    }

    [HttpPost("batch/remove")]
    public INotifyResult<IMessageObject> BatchDelete([FromBody] BatchRemoveParams param)
    {
      var msg = "数据已批量删除";
      var entities = _context.Set<TEntity>()
        .Where(entity => param.Ids.Contains(entity.Id))
        .ToArray();

      _context.Remove(entities);
      _context.SaveChanges();
      _logger.LogInformation(msg);

      return NotifyResult.FromVoid().Success(msg);
    }
  }

  public abstract class EntityController<TEntity>: EntityController<TEntity, long>
    where TEntity: class, IEntity<long>
  {
    public EntityController(
      DomainContext context,
      ILogger<TEntity> logger
    ): base(context, logger) {}
  }
}
