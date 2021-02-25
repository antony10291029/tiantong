using System;

namespace Namei.Wcs.Api
{
  public class DeviceState
  {
    public int id { get; set; }

    public string type { get; set; }

    public int device_id { get; set; }

    public string state { get; set; }

    public DateTime started_at { get; set; } = DateTime.Now;

    public DateTime ended_at { get; set; } = DateTime.MinValue;
  }
}
