namespace Wcs.Plc
{
  public class Container: IContainer
  {
    static public IContainer GetTestContainer()
    {
      return new Container {
        Event = new Event(),
        StateDriver = new StateTestDriver(),
        IntervalManager = new IntervalManager()
      };
    }

    public IEvent Event { get; set; }

    public IStateDriver StateDriver { get; set; }

    public IIntervalManager IntervalManager { get; set; }
  }
}
