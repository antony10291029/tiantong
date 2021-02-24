using System;
using Microsoft.AspNetCore.Mvc;
using Midos.Center.Entities;
using System.Linq;

namespace Midos.Center.Controllers
{
  public class ConfigController: BaseController
  {
    private DomainContext _domain;

    public ConfigController(DomainContext domain)
    {
      _domain = domain;
    }

    [HttpPost("/midos/configs")]
    public IResult<Config[]> Configs()
    {
      return Result.From(_domain.Configs.ToArray());
    }

    public class ConfigParams
    {
      public string Key { get; set; }

      public string Value { get; set; }
    }

    [HttpPost("/midos/configs/create")]
    public INotifyResult<MessageObject> Create([FromBody] ConfigParams[] param)
    {
      _domain.Configs.AddRange(param.Select(item => new Config {
        Key = item.Key,
        Value = item.Value,
        UpdatedAt = DateTime.Now
      }));

      _domain.SaveChanges();

      return NotifyResult.FromVoid().Success("配置已创建");
    }

    [HttpPost("/midos/configs/update")]
    public INotifyResult<MessageObject> Update([FromBody] ConfigParams[] param)
    {
      _domain.Configs.UpdateRange(param.Select(item => new Config {
        Key = item.Key,
        Value = item.Value,
        UpdatedAt = DateTime.Now,
      }));
      _domain.SaveChanges();

      return NotifyResult.FromVoid().Success("配置已更新");
    }

    public class RemoveParams
    {
      public string[] Keys { get; set; }
    }

    [HttpPost("/midos/configs/delete")]
    public INotifyResult<MessageObject> Remove([FromBody] RemoveParams param)
    {
      var items = _domain.Configs
        .Where(config => param.Keys.Contains(config.Key))
        .ToArray();

      _domain.Configs.RemoveRange(items);
      _domain.SaveChanges();

      return NotifyResult.FromVoid().Success("配置已删除");
    }
  }
}
