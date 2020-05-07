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

    public AppController(Config config, IRandom random)
    {
      _config = config;
      _random = random;
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
    [Route("test")]
    public object Test()
    {
      var plc = new Plc {
        id = 1,
        name = "test",
        model = PlcModel.S7200Smart,
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
                plc_id = 1,
                state_id = 1,
                pusher_id = 1,
                pusher = new HttpPusher {
                  when_opt = "",
                  when_value = "",
                  url = "http://localhost:5100/",
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
                plc_id = 2,
                state_id = 2,
                pusher_id = 2,
                pusher = new HttpPusher {
                  when_opt = "",
                  when_value = "",
                  url = "http://localhost:5100/",
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

      Task.Run(() => worker.Run());

      return SuccessOperation("Plc 运行中");
    }

  }
}
