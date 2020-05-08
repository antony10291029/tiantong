using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Renet;
using Renet.Web;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  [Route("/")]
  public class AppController: BaseController
  {
    private Config _config;

    private IRandom _random;

    private PlcManager _plcManager;

    public AppController(Config config, IRandom random, PlcManager plcManager)
    {
      _config = config;
      _random = random;
      _plcManager = plcManager;
    }

    [HttpGet]
    [HttpPost]
    public object Home()
    {
      return new {
        message = _config.AppName,
        version = _config.AppVersion,
      };
    }

    [HttpPost]
    [Route("data")]
    public object Data([FromBody] object data)
    {
      return data;
    }

    [HttpPost]
    [Route("test")]
    public object TestRun()
    {
      
      var plc = new Plc {
        id = 1,
        name = "test",
        model = PlcModel.Test,
        host = "192.168.20.10",
        port = 102,
        comment = "test plc comment",
        states = new List<PlcState> {
          new PlcState {
            id = 1,
            plc_id = 1,
            type = StateType.UInt16,
            length = 1,
            name = "心跳",
            address = "D1.100",
            is_heartbeat = true,
            heartbeat_interval = 1000,
            heartbeat_max_value = 10000,
            is_collect = true,
            collect_interval = 1000,
            state_http_pushers = new List<PlcStateHttpPusher> {
              new PlcStateHttpPusher {
                id = 1,
                state_id = 1,
                pusher_id = 1,
                pusher = new HttpPusher {
                  when_opt = "",
                  when_value = "",
                  url = "http://localhost:5000/data",
                  value_key = "heartbeat",
                  data = "{\"plc\": 1}",
                  to_string = false,
                }
              }
            }
          },
          new PlcState {
            id = 2,
            plc_id = 1,
            type = StateType.String,
            length = 10,
            name = "扫码器",
            address = "D1.120",
            is_heartbeat = false,
            heartbeat_interval = 0,
            heartbeat_max_value = 0,
            is_collect = true,
            collect_interval = 1000,
            state_http_pushers = new List<PlcStateHttpPusher> {
              new PlcStateHttpPusher {
                id = 2,
                state_id = 2,
                pusher_id = 2,
                pusher = new HttpPusher {
                  when_opt = "",
                  when_value = "",
                  url = "http://localhost:5000/data",
                  value_key = "scanner",
                  data = "{\"plc\": 1}",
                  to_string = false,
                }
              }
            }
          }
        }
      };

      var worker = PlcBuilder.Build(plc);

      worker.UShort("心跳").Watch(_ => {
        worker.String("扫码器").Set(_random.String(10));
      });

      _plcManager.Run(worker);

      return SuccessOperation("Plc 运行中");
    }

    [HttpPost]
    [Route("test/stop")]
    public object TestStop()
    {
      _plcManager.Stop(1);

      return SuccessOperation("PLC 已停止");
    }

  }
}
