using Microsoft.AspNetCore.Mvc;
using Renet;
using Renet.Web;
using System;
using System.Linq;
using Z.EntityFramework.Plus;

namespace Namei.Wcs.Api
{
  public class DeviceController: BaseController
  {
    private DomainContext _domain;

    public DeviceController(DomainContext domain)
    {
      _domain = domain;
    }

    [HttpPost("/devices/add")]
    public object Add([FromBody] Device device)
    {
      device.created_at = DateTime.Now;
      device.is_enable = true;

      _domain.Add(device);
      _domain.SaveChanges();

      return new { message = "设备已添加" };
    }

    public class RemoveParams
    {
      public int device_id { get; set; }
    }

    [HttpPost("/devices/remove")]
    public object Remove([FromBody] RemoveParams param)
    {
      var device = _domain.Devices
        .Where(d => d.id == param.device_id)
        .FirstOrDefault();

      _domain.Remove(device);
      _domain.SaveChanges();
      
      return new { message = "设备已删除" };
    }

    [HttpPost("/devices/update")]
    public object Update([FromBody] Device device)
    {
      var oldData = _domain.Devices.FirstOrDefault(d => d.id == device.id);

      if (oldData == null) {
        throw KnownException.Error("设备不存在", 400);
      }

      _domain.Entry(oldData).CurrentValues.SetValues(device);
      _domain.Entry(oldData).Property(e => e.created_at).IsModified = false;

      return new { message = "设备已更新" };
    }

    [HttpPost("/devices/all")]
    public object All()
    {
      return _domain.Devices.ToArray();
    }
  }
}
