using Microsoft.AspNetCore.Mvc;
using Renet.Utils;
using Renet.Web;
using System.Collections.Generic;
using System.Linq;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  [Route("/dev")]
  public class DevController: BaseController
  {
    private IRandom _random;

    private SystemContext _system;

    private DomainContextFactory _domain;

    public DevController(
      IRandom random,
      SystemContext system,
      DomainContextFactory domain
      
    ) {
      _random = random;
      _system = system;
      _domain = domain;
    }

    [HttpPost]
    [Route("migrate")]
    public object Migrate()
    {
      _domain.Migrate();

      return SuccessOperation("数据库已迁移");
    }

    [HttpPost]
    [Route("rollback")]
    public object Rollback()
    {
      _domain.Rollback();

      return SuccessOperation("数据库已回档");
    }

    [HttpPost]
    [Route("refresh")]
    public object Refresh()
    {
      _domain.Refresh();

      return SuccessOperation("数据库已刷新");
    }

    [HttpPost]
    [Route("seed")]
    public object Seed()
    {
      InsertPlcData();
      return SuccessOperation("数据已插入");
    }

    [HttpPost]
    [Route("reseed")]
    public object Reseed()
    {
      _domain.Refresh();
      InsertPlcData();
      return SuccessOperation("数据已重新插入");
    }

    private void InsertPlcData()
    {
      _random.For(10, 20, i => {
        _system.Add(new Plc {
          name = $"测试设备 {i}",
          number = $"{i}",
          comment = $"测试设备 {i}",
          host = "localhost",
          port = 8000,
          model = PlcModel.Test,
          states = _random.Enumerate(1, 10).Select(j => new PlcState {
            name = $"测试数据点 {j}",
            number = $"{j}",
            type = _random.Bool() ? PlcStateType.String : PlcStateType.UInt16,
            address = $"D{_random.Int(1000, 10000)}",
            length = 10,
            is_heartbeat = _random.Bool(),
            heartbeat_interval = 1000,
            heartbeat_max_value = 10000,
            is_collect = _random.Bool(),
            collect_interval = 1000,
            state_http_pushers = _random.Bool() ? new List<PlcStateHttpPusher>{} : new List<PlcStateHttpPusher> {
              new PlcStateHttpPusher {
                pusher = new HttpPusher {
                  name = "测试推送",
                  number = "1",
                  url = "http://localhost:5000/data",
                  field = "value",
                  to_string = false,
                  header = "{}",
                  body = "{\"field\": 1}"
                }
              }
            }
          }).ToList()
        });
      });

      _system.SaveChanges();
    }
  }
}