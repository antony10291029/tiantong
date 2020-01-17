namespace Wcs.Plc
{
  public interface IPlcContainer
  {
    IPlc Plc { get; set; }

    IEvent Event { get; }

    IStateDriver StateDriver { get; }

    IStateManager StateManager { get; }

    IIntervalManager IntervalManager { get; }
  }
}
