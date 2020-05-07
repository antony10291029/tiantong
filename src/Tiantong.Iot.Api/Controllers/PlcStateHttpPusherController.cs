using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  [Route("/plcs/states/http/pushers")]
  public class PlcStateHttpPusherController: BaseController
  {
    private PlcStateHttpPusherRepository _pusherRepository;

    public PlcStateHttpPusherController(PlcStateHttpPusherRepository pusherRepository)
    {
      _pusherRepository = pusherRepository;
    }

    public class CreateParams
    {
      public int state_id { get; set; }

      public HttpPusher pusher { get; set; }
    }

    [HttpPost]
    [Route("create")]
    public object Create([FromBody] CreateParams param)
    {
      _pusherRepository.Add(param.state_id, param.pusher);

      return SuccessOperation("PLC数据点已创建");
    }

    public class DeleteParams
    {
      public int state_id { get; set; }

      public int pusher_id { get; set; }
    }

    [HttpPost]
    [Route("delete")]
    public object Delete([FromBody] DeleteParams param)
    {
      _pusherRepository.Delete(param.state_id, param.pusher_id);

      return SuccessOperation("PLC数据点已删除");
    }

    [HttpPost]
    [Route("update")]
    public object Update([FromBody] HttpPusher pusher)
    {
      _pusherRepository.Update(pusher);

      return SuccessOperation("PLC数据点已更新");
    }

  }
}
