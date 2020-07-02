using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace Yuchuan.IErp.Api
{
  public class DeviceStateHub: Hub
  {
    private DomainContext _domain;

    private DeviceStateRepository _deviceStateRepository;

    public DeviceStateHub(
      DomainContext domain,
      DeviceStateRepository deviceStateRepository
    ) {
      _domain = domain;
      _deviceStateRepository = deviceStateRepository;
    }

    public async Task UseProject(int projectId)
    {
      var group = $"device_states:{projectId}";
      var ids = _domain.ProjectDevices
        .Where(pd => pd.project_id == projectId)
        .ToArray()
        .Select(pd => pd.device_id);
      var devices = _domain.Devices
        .Where(d => ids.Contains(d.id))
        .OrderBy(d => d.id)
          .ThenBy(d => d.id)
        .ToArray();
      var states = ids.Select(id => _deviceStateRepository.GetCurrentState(id));

      await Clients.Caller.SendAsync("initialize", new { devices, states });
      await Groups.AddToGroupAsync(Context.ConnectionId, group);
    }
  }
}
