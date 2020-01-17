namespace Wcs.Plc
{
  public class PlcContainer : IPlcContainer
  {
    public IPlc Plc { get; set; }

    public IEvent Event { get; set; }

    public IStateDriver StateDriver { get; set; }

    public IStateManager StateManager { get; set; }

    public IIntervalManager IntervalManager { get; set; }

    public PlcContainer()
    {
      Event = new Event();
      StateDriver = new StateTestDriver();
      StateManager = new StateManager(this);
      IntervalManager = new IntervalManager();
    }
  }
}
