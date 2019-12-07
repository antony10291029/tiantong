namespace Wcs.Plc
{
  public interface IContainer
  {
    IStateDriver StateDriver { get; }

    IIntervalManager IntervalManager { get; }
  }
}
