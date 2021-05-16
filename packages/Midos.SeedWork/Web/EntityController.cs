using Microsoft.AspNetCore.Mvc;

namespace Midos.SeedWork.Api
{
  public class EntityController<TEntity>: ControllerBase
    where TEntity: IEntity
  {
    private readonly IRepository<TEntity> _repository;

    public EntityController(IRepository<TEntity> repository)
    {
      _repository = repository;
    }

    [HttpPost("add")]
    public INotifyResult<IMessageObject> Add([FromBody] TEntity entity)
    {
      _repository.Add(entity);

      return NotifyResult.FromVoid().Success("数据已创建");
    }

    [HttpPost("add-range")]
    public INotifyResult<IMessageObject> AddRange([FromBody] TEntity[] entities)
    {
      _repository.AddRange(entities);

      return NotifyResult.FromVoid().Success("数据已创建");
    }

    public record RemoveParams
    {
      public long Id { get; set; }
    }

    [HttpPost("remove")]
    public INotifyResult<IMessageObject> Remove([FromBody] RemoveParams param)
    {
      _repository.Remove(param.Id);

      return NotifyResult.FromVoid().Success("数据已删除");
    }

    public record RemoveRangeParams
    {
      public long[] Ids { get; set; }
    }

    [HttpPost("remove-range")]
    public INotifyResult<IMessageObject> RemoveRange([FromBody] RemoveRangeParams parma)
    {
      _repository.RemoveRange(parma.Ids);

      return NotifyResult.FromVoid().Success("数据已删除");
    }

    [HttpPost("update")]
    public INotifyResult<IMessageObject> Update([FromBody] TEntity entity)
    {
      _repository.Update(entity);

      return NotifyResult.FromVoid().Success("数据已更新");
    }

    [HttpPost("update-range")]
    public INotifyResult<IMessageObject> UpdateRange([FromBody] TEntity[] entities)
    {
      _repository.UpdateRange(entities);

      return NotifyResult.FromVoid().Success("数据已更新");
    }

    public record FindParams
    {
      public long Id { get; set; }
    }

    [HttpPost("find")]
    public TEntity Find([FromBody] FindParams param)
      => _repository.Find(param.Id);

    public record FindRangeParams
    {
      public long[] Ids { get; set; }
    }

    [HttpPost("find-range")]
    public DataMap<TEntity> FindRange([FromBody] FindRangeParams param)
      => _repository.FindRange(param.Ids);

    [HttpPost("query")]
    public DataMap<TEntity> Query([FromBody] QueryParams param)
      => _repository.Query(param);

    [HttpPost("paginate")]
    public Pagination<TEntity> Paginate([FromBody] PaginateParams param)
      => _repository.Paginate(param);
  }
}
