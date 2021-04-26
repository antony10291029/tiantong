using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Wcs.Api
{
  [Table("device_errors")]
  public class DeviceError
  {
    public int id { get; set; }

    public int device_id { get; set; }

    public string error { get; set; }

    public string message { get; set; }

    public DateTime error_at { get; set; } = DateTime.Now;

    public DateTime recovered_at { get; set; } = DateTime.MinValue;
  }
}
