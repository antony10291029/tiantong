namespace Wcs.Plc
{
  public interface IContainer
  {
    IEvent Event { get; }

    IStateDriver StateDriver { get; }

    IIntervalManager IntervalManager { get; }
  }
}
