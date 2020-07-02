using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Renet;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Yuchuan.IErp.Api
{
  public class DeviceStateRepository
  {
    private DomainContext _domain;

    private IDistributedCache _cache;

    private IHubContext<DeviceStateHub>  _stateHub;

    public DeviceStateRepository(
      DomainContext domain,
      IDistributedCache cache,
      IHubContext<DeviceStateHub> stateHub
    ) {
      _domain = domain;
      _cache = cache;
      _stateHub = stateHub;
    }

    public DeviceState GetCurrentState(int deviceId)
    {
      var data = _cache.GetString($"device_states:{deviceId}");

      if (data is null) {
        var state = _domain.DeviceStates
          .Where(ds => ds.device_id == deviceId)
          .OrderByDescending(ds => ds.created_at)
          .FirstOrDefault();

        if (state is null) {
          throw KnownException.Error("设备不存在", 404);
        }

        _cache.SetString($"device_states:{deviceId}", JsonSerializer.Serialize(state));

        return state;
      } else {
        return JsonSerializer.Deserialize<DeviceState>(data);
      }
    }

    public void SetCache(DeviceState state)
    {
      _cache.SetString($"device_states:{state.device_id}", JsonSerializer.Serialize(state));
    }

    public void Snapshot(DeviceState state)
    {
      var now = DateTime.Now;

      if (state.created_at < now.AddSeconds(-10)) {
        state.state = "未知";
      }

      state.id = 0;
      state.created_at = DateTime.Now;

      _domain.Add(state);
      _domain.SaveChanges();
    }

    public async Task CommitAsync(int deviceId, string key, string value)
    {
      var projectId = _domain.ProjectDevices
        .FirstOrDefault(pd => pd.device_id == deviceId)
        ?.project_id;

      if (projectId is null) {
        throw KnownException.Error("项目不存在", 404);
      }

      var state = GetCurrentState(deviceId);
      var isChanged = false;

      switch (key) {
        case "state":
          isChanged = state.state != value;
          state.state = value;
          break;
        case "mode":
          isChanged = state.mode != value;
          state.mode = value;
          break;
        case "position":
          isChanged = state.position != value;
          state.position = value;
          break;
        case "message":
          isChanged = state.message != value;
          state.message = value;
          break;
        default: throw KnownException.Error("状态 key 不存在");
      }

      if (key == "state" && isChanged) {
        Snapshot(state);
      }

      state.created_at = DateTime.Now;
      SetCache(state);

      await _stateHub.Clients.Group($"device_states:{projectId}").SendAsync("commit", new {
        device_id = deviceId,
        key = key,
        value = value
      });
    }
  }
}
