namespace Wcs.Plc
{
  public interface IPlc : IPlcWorker
  {
    IEvent Event { get; }

    IContainer Container { get; }

    IStateManager StateManager { get; }

    IIntervalManager IntervalManager { get; }
  }
}
