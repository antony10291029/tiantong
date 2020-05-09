using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DBCore;
using Renet;
using Renet.Web;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  [Route("/dev")]
  public class DevController: BaseController
  {
    private IMigrator _mg;

    private IRandom _random;

    private IotDbContext _db;

    public DevController(
      IMigrator mg,
      IRandom random,
      IotDbContext db
    ) {
      _mg = mg;
      _db = db;
      _random = random;
    }

    [HttpPost]
    [Route("migrate")]
    public object Migrate()
    {
      _mg.Migrate();

      return SuccessOperation("数据库已迁移");
    }

    [HttpPost]
    [Route("rollback")]
    public object Rollback()
    {
      _mg.Rollback();

      return SuccessOperation("数据库已回档");
    }

    [HttpPost]
    [Route("refresh")]
    public object Refresh()
    {
      _mg.Refresh();

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
      _mg.Refresh();
      InsertPlcData();
      return SuccessOperation("数据已重新插入");
    }

    private void InsertPlcData()
    {
      _random.For(1, 10, i => {
        _db.Add(new Plc {
          name = $"测试设备 {i}",
          comment = $"测试设备 {i}",
          host = "localhost",
          port = 8000,
          model = PlcModel.Test,
          states = _random.Enumerate(1, 10).Select(j => new PlcState {
            name = $"测试数据点 {j}",
            type = _random.Bool() ? StateType.String : StateType.UInt16,
            address = $"D{_random.Int(1000, 10000)}",
            is_heartbeat = false,
            is_collect = true,
            collect_interval = 1000,
            state_http_pushers = _random.Bool() ? new List<PlcStateHttpPusher>{} : new List<PlcStateHttpPusher> {
              new PlcStateHttpPusher {
                pusher = new HttpPusher {
                  name = "测试推送",
                  url = "http://localhost:5000/data",
                  when_opt = "",
                  when_value = "",
                  value_key = "field",
                  data = "{\"field\": 1}",
                  to_string = false,
                }
              }
            }
          }).ToList()
        });
      });

      _db.SaveChanges();
    }
  }
}
