using Wcs.Plc.Database;
using Wcs.Plc.Entities;

namespace Wcs.Plc
{
  public interface IPlcContainer
  {
    IPlc Plc { get; set; }

    IEvent Event { get; }

    IStateDriver StateDriver { get; }

    IStateManager StateManager { get; }

    IStatePlugin StateLogger { get; }

    PlcConnection PlcConnection { get; }

    IIntervalManager IntervalManager { get; }

    IStateDriverProvider StateDriverProvider { get; }

    DbContext ResolveDbContext();

    void ResolvePlcConnection();
  }
}
