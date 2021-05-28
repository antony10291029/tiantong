using System;
using System.Linq;

namespace Namei.Wcs.Api
{
  public class DeviceErrorService
  {
    private readonly DomainContext _domain;

    public DeviceErrorService(DomainContext domain)
    {
      _domain = domain;
    }

    public void Log(string deviceKey, string code, string message = "")
    {
      var device = _domain.Devices.FirstOrDefault(d => d.key == deviceKey);

      if (device == null) {
        throw KnownException.Error("设备不存在");
      }

      var error = _domain.DeviceErrors
        .Where(de => de.device_id == device.id && de.recovered_at == DateTime.MinValue)
        .FirstOrDefault();

      if (error != null && error.error != code) {
        error.recovered_at = DateTime.Now;
        _domain.SaveChanges();
        error = null;
      }

      if (error == null && code != "0") {
        if (message == "") {
          message = "发生异常，请查看相关硬件";
        }

        _domain.Add(new DeviceError {
          device_id = device.id,
          error = code.ToString(),
          message = message,
          error_at = DateTime.Now,
          recovered_at = DateTime.MinValue,
        });
      }

      _domain.SaveChanges();
    }
  }
}
