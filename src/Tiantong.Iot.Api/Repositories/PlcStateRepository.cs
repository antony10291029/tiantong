using System.Linq;
using Tiantong.Iot.Entities;
using Renet.Web;

namespace Tiantong.Iot.Api
{
  public class PlcStateRepository
  {
    private LogContext _log;

    private SystemContext _system;

    public PlcStateRepository(LogContext log, SystemContext system)
    {
      _log = log;
      _system = system;
    }

    public void Add(PlcState state)
    {
      _system.PlcStates.Add(state);
      _system.SaveChanges();
    }

    public void Delete(int id)
    {
      var state = _system.PlcStates.FirstOrDefault(s => s.id == id);

      if (state == null) {
        throw new FailureOperation("PLC 数据点不存在");
      }

      _system.PlcStates.Remove(state);
      _system.SaveChanges();
    }

    public void Update(PlcState state)
    {
      var oldState = _system.PlcStates.FirstOrDefault(s => s.id == state.id);

      if (oldState == null) {
        throw new FailureOperation("PlC 数据点不存在");
      }

      _system.Entry(oldState).CurrentValues.SetValues(state);
      _system.Entry(oldState).Property(oldstate => oldstate.plc_id).IsModified = false;
      _system.SaveChanges();
    }

    public PlcState Find(int stateId)
    {
      var state = _system.PlcStates.FirstOrDefault(ps => ps.id == stateId);

      if (state == null) {
        throw new FailureOperation("PLC 数据点不存在");
      }

      return state;
    }

    public PlcState[] All(int plcId)
    {
      return _system.PlcStates
        .Where(s => s.plc_id == plcId)
        .OrderBy(s => s.id)
        .ToArray();
    }

  }
}
