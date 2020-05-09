using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Iot.Api
{
  [Route("/plcs/workers")]
  public class PlcWorkerController: BaseController
  {
    private PlcManager _plcManager;

    private PlcRepository _plcRepository;

    public PlcWorkerController(
      PlcManager plcManager,
      PlcRepository plcRepository
    ) {
      _plcManager = plcManager;
      _plcRepository = plcRepository;
    }


    public class RunParams
    {
      public int id { get; set; }
    }

    [HttpPost]
    [Route("run")]
    public object Run([FromBody] RunParams param)
    {
      var plc = _plcRepository.EnsureFind(param.id);
      var worker = PlcBuilder.Build(plc);

      _plcManager.Add(worker);

      return SuccessOperation("PLC 开始运行");
    }

    [HttpPost]
    [Route("stop")]
    public object Stop([FromBody] RunParams param)
    {
      _plcManager.Stop(param.id);

      return SuccessOperation("PLC 停止运行");
    }
  }
}
