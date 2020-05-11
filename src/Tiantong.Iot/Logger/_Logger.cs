namespace Tiantong.Iot
{
  public abstract class Logger
  {
    public Logger(IntervalManager manager, int interval = 500)
    {
      manager.Add(new Interval(HandleLog, interval));
    }

    public abstract void HandleLog();
  }
}
