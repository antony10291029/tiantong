using System;
using Microsoft.AspNetCore.Mvc;
using Midos.Center.Entities;
using System.Linq;
using Midos.Domain;

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
      var configs = _domain.Set<Config>()
        .OrderBy(config => config.Key)
        .ToArray();

      return Result.From(configs);
    }

    public class ConfigParams
    {
      public string Key { get; set; }

      public string Value { get; set; }
    }

    [HttpPost("/midos/configs/create")]
    public INotifyResult<IMessageObject> Create([FromBody] ConfigParams[] param)
    {
      _domain.Set<Config>().AddRange(
        param.Select(item => new Config {
          Key = item.Key,
          Value = item.Value,
          UpdatedAt = DateTime.Now
        })
      );

      _domain.SaveChanges();

      return NotifyResult.FromVoid().Success("配置已创建");
    }

    [HttpPost("/midos/configs/update")]
    public INotifyResult<IMessageObject> Update([FromBody] ConfigParams[] param)
    {
      _domain.Set<Config>().UpdateRange(
        param.Select(item => new Config {
          Key = item.Key,
          Value = item.Value,
          UpdatedAt = DateTime.Now,
        })
      );
      _domain.SaveChanges();

      return NotifyResult.FromVoid().Success("配置已更新");
    }

    public class RemoveParams
    {
      public string[] Keys { get; set; }
    }

    [HttpPost("/midos/configs/delete")]
    public INotifyResult<IMessageObject> Remove([FromBody] RemoveParams param)
    {
      var items = _domain.Set<Config>()
        .Where(config => param.Keys.Contains(config.Key))
        .ToArray();

      _domain.RemoveRange(items);
      _domain.SaveChanges();

      return NotifyResult.FromVoid().Success("配置已删除");
    }
  }
}
