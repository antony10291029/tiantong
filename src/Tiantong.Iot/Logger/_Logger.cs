using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public abstract class Logger: ILogger
  {
    protected IotDbContext DbContext;

    // 利用 public 引入 DbContext 解决资源释放问题
    public void UseDbContext(IotDbContext dbContext)
    {
      DbContext = dbContext;
    }

    // IntervalManager 依赖应在构造函数中处理
    protected Logger UseIntervalManager(IntervalManager manager, int interval = 500)
    {
      manager.Add(new Interval(HandleLog, interval));

      return this;
    }

    public virtual void HandleLog()
    {

    }

  }

}
