using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Namei.Wcs.Api
{
  public class DoorWebCommandController: BaseController
  {
    private DoorServiceManager _doors;

    public DoorWebCommandController(DoorServiceManager doors)
    {
      _doors = doors;
    }

    public class OpenParams
    {
      public string DoorId { get; set; }
    }

    [HttpPost("/doors/open")]
    public object Open([FromBody] OpenParams param)
    {
      _doors.Get(param.DoorId).Open();

      return new { message = "开门命令已执行" };
    }
  }
}