using System;
using System.Linq;

namespace Namei.Wcs.Api
{
  public class DeviceStateLogger
  {
    private readonly DomainContext _domain;

    public DeviceStateLogger(DomainContext domain)
    {
      _domain = domain;
    }

    private void Log(string _, int deviceId, string state)
    {
      var log = _domain.DeviceStates.FirstOrDefault(ls =>
        ls.device_id == deviceId &&
        ls.type == DeviceType.Lifter &&
        ls.ended_at == DateTime.MinValue
      );

      if (log is null || log.state != state) {
        _domain.Add(new DeviceState {
          type = DeviceType.Lifter,
          device_id = deviceId,
          state = state,
        });
      } else if (log?.state != state) {
        log.ended_at = DateTime.Now;
        _domain.Add(new DeviceState {
          type = DeviceType.Lifter,
          device_id = deviceId,
          state = state,
        });
      } else {
        // 状态未发生改变，不做记录

        return;
      }

      _domain.SaveChanges();
    }

    public void LogDoor(int doorId, string state)
      => Log(DeviceType.Door, doorId, state);

    public void LogLifter(int lifterId, string state)
      => Log(DeviceType.Lifter, lifterId, state);

  }
}
