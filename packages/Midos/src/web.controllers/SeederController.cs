namespace Microsoft.AspNetCore.Mvc
{
  public abstract class SeederControllerBase
  {
    protected abstract void Seed();

    protected abstract void Reseed();

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
      Reseed();

      return NotifyResult
        .FromVoid()
        .Success("数据已重新插入");
    }
  }
}
