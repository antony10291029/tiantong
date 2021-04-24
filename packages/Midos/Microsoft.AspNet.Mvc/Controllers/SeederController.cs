using DBCore;

namespace Microsoft.AspNetCore.Mvc
{
  public abstract class SeederControllerBase
  {
    private IMigrator _migrator;

    public SeederControllerBase(IMigrator migrator)
    {
      _migrator = migrator;
    }

    protected abstract void Seed();

    [HttpPost("/midos/seeder/seed")]
    public INotifyResult<IMessageObject> HandleSeed()
    {
      Seed();

      return NotifyResult
        .FromVoid()
        .Success("数据已全部插入");
    }

    [HttpPost("/midos/seeder/reseed")]
    public INotifyResult<IMessageObject> HandleReseed()
    {
      _migrator.Refresh();

      Seed();

      return NotifyResult
        .FromVoid()
        .Success("数据已重新插入");
    }
  }
}
