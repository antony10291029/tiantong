using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  [Route("/plcs/states")]
  public class PlcStateController: BaseController
  {
    private PlcStateRepository _stateRepository;

    public PlcStateController(PlcStateRepository stateRepository)
    {
      _stateRepository = stateRepository;
    }

    [HttpPost]
    [Route("create")]
    public object Create([FromBody] PlcState state)
    {
      _stateRepository.Add(state);

      return SuccessOperation("PLC数据点已创建");
    }

    public class DeleteParams
    {
      public int id { get; set; }
    }

    [HttpPost]
    [Route("delete")]
    public object Delete([FromBody] DeleteParams param)
    {
      _stateRepository.Delete(param.id);

      return SuccessOperation("PLC数据点已删除");
    }

    [HttpPost]
    [Route("update")]
    public object Update([FromBody] PlcState state)
    {
      _stateRepository.Update(state);

      return SuccessOperation("PLC数据点已更新");
    }

    public class AllParams
    {
      public int plc_id { get; set; }
    }

    [HttpPost]
    [Route("all")]
    public PlcState[] All([FromBody] AllParams param)
    {
      return _stateRepository.All(param.plc_id);
    }

  }
}
