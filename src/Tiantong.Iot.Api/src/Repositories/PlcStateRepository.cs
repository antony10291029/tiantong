using Microsoft.EntityFrameworkCore;
using Renet;
using System.Linq;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class PlcStateRepository
  {
    private LogContext _log;

    private SystemContext _system;

    private PlcRepository _plcRepository;

    public PlcStateRepository(LogContext log, SystemContext system, PlcRepository plcRepository)
    {
      _log = log;
      _system = system;
      _plcRepository = plcRepository;
    }

    public void Add(PlcState state)
    {
      state.id = 0;
      if (_system.PlcStates.Any(s => s.plc_id == state.plc_id && s.name == state.name)) {
        throw KnownException.Error("数据点名称已存在");
      }

      var plc = _plcRepository.EnsureGet(state.plc_id);

      PlcState.EnsureAddress(plc.model, state.address);

      _system.PlcStates.Add(state);
      _system.SaveChanges();
    }

    public void Delete(int id)
    {
      var state = _system.PlcStates
        .Include(s => s.state_http_pushers)
          .ThenInclude(s => s.pusher)
        .FirstOrDefault(s => s.id == id);

      if (state == null) {
        throw KnownException.Error("PLC 数据点不存在");
      }

      _system.PlcStates.Remove(state);
      _system.SaveChanges();
    }

    public void Update(PlcState state)
    {
      var oldState = _system.PlcStates.FirstOrDefault(s => s.id == state.id);

      if (oldState == null) {
        throw KnownException.Error("PlC 数据点不存在");
      }

      if (_system.PlcStates.Any(s => s.id != state.id && s.name == state.name)) {
        throw KnownException.Error("数据点名称已存在");
      }

      var plc = _plcRepository.EnsureGet(state.plc_id);

      PlcState.EnsureAddress(plc.model, state.address);

      _system.Entry(oldState).CurrentValues.SetValues(state);
      _system.Entry(oldState).Property(oldstate => oldstate.plc_id).IsModified = false;
      _system.SaveChanges();
    }

    public PlcState Find(int stateId)
    {
      var state = _system.PlcStates.FirstOrDefault(ps => ps.id == stateId);

      if (state == null) {
        throw KnownException.Error("PLC 数据点不存在");
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