using System;
using System.ComponentModel.DataAnnotations;

namespace Namei.Wcs.Api
{
  public class WcsDoorPassport
  {
    [Key]
    public string Id { get; private set; }

    public DateTime ExpiredAt { get; private set; }

    private WcsDoorPassport() {}

    public void SetExpiredAt(DateTime time)
    {
      ExpiredAt = time;
    }

    public void AddMilliseconds(int time)
    {
      ExpiredAt = DateTime.Now.AddMilliseconds(time);
    }

    public static WcsDoorPassport From(string Id, int time)
      => new WcsDoorPassport() {
        Id = Id,
        ExpiredAt = DateTime.Now.AddMilliseconds(time),
      };
  }
}
