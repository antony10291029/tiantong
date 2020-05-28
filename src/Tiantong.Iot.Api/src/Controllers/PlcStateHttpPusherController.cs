using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  [Route("/plcs/states/http-pushers")]
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

      return SuccessOperation("HTTP 推送已创建");
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

    public class UpdateParams
    {
      public int state_id { get; set; }

      public HttpPusher pusher { get; set; }
    }

    [HttpPost]
    [Route("update")]
    public object Update([FromBody] UpdateParams param)
    {
      _pusherRepository.Update(param.state_id, param.pusher);

      return SuccessOperation("PLC数据点已更新");
    }

    public class AllParams
    {
      public int state_id { get; set; }
    }

    [HttpPost]
    [Route("all")]
    public HttpPusher[] All([FromBody] AllParams param)
    {
      return _pusherRepository.All(param.state_id);
    }

  }
}
