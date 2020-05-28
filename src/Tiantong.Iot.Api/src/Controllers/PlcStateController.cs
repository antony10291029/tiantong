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

      return SuccessOperation("PLC数据点已创建", state.id);
    }

    public class FindParams
    {
      public int state_id { get; set; }
    }

    [HttpPost]
    [Route("delete")]
    public object Delete([FromBody] FindParams param)
    {
      _stateRepository.Delete(param.state_id);

      return SuccessOperation("PLC数据点已删除");
    }

    [HttpPost]
    [Route("update")]
    public object Update([FromBody] PlcState state)
    {
      _stateRepository.Update(state);

      return SuccessOperation("PLC数据点已更新");
    }

    [HttpPost]
    [Route("find")]
    public object Find([FromBody] FindParams param)
    {
      return _stateRepository.Find(param.state_id);
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
