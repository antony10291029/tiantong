using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tiantong.Iot.Entities;
using Renet.Web;

namespace Tiantong.Iot.Api
{
  public class PlcStateHttpPusherRepository
  {
    private LogContext _log;

    private SystemContext _system;

    public PlcStateHttpPusherRepository(LogContext log, SystemContext system)
    {
      _log = log;
      _system = system;
    }

    public void Add(int stateId, HttpPusher pusher)
    {
      var statePusher = new PlcStateHttpPusher {
        state_id = stateId,
        pusher = pusher,
      };

      _system.Add(statePusher);
      _system.SaveChanges();
    }

    public void Delete(int stateId, int pusherId)
    {
      var statePusher = _system.PlcStateHttpPushers
        .Include(sp => sp.pusher)
        .FirstOrDefault(sp => sp.state_id == stateId && sp.pusher_id == pusherId);

      if (statePusher == null) {
        throw new FailureOperation("PLC 数据点不存在");
      }

      _system.Remove(statePusher);
      _system.SaveChanges();
    }

    public void Update(int stateId, HttpPusher pusher)
    {
      var oldPusher = _system.HttpPushers.FirstOrDefault(s => s.id == pusher.id);

      if (oldPusher == null) {
        throw new FailureOperation("HTTP推送不存在");
      }

      _system.Entry(oldPusher).CurrentValues.SetValues(pusher);
      _system.SaveChanges();
    }

    public HttpPusher[] All(int stateId)
    {
      return _system.PlcStateHttpPushers
        .Include(state => state.pusher)
        .Where(shp => shp.state_id == stateId)
        .Select(shp => shp.pusher)
        .ToArray();
    }

  }

}
