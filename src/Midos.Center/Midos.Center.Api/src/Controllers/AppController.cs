﻿using Microsoft.AspNetCore.Mvc;
using Midos.Center.Entities;
using System.Linq;
using Midos.Domain;

namespace Midos.Center.Controllers
{
  public class AppController: BaseController
  {
    private readonly DomainContext _domain;

    public AppController(
      DomainContext domain
    ) {
      _domain = domain;
    }

    public class CreateParams
    {
      public string Class { get; set; } = "default";

      public string Key { get; set; }

      public string Name { get; set; }

      public string Url { get; set; }
    }

    public class CreateResult: MessageObject
    {
      public long Id { get; set; }
    }

    [HttpPost("/midos/apps/create")]
    public INotifyResult<CreateResult> Create([FromBody] CreateParams param)
    {
      var app = App.From(
        klass: param.Class,
        key: param.Key,
        name: param.Key,
        url: param.Url
      );

      _domain.Set<App>().Add(app);
      _domain.SaveChanges();

      return NotifyResult
        .From(new CreateResult { Id = app.Id })
        .Success("应用已创建")
        .StatusCode(201);
    }

    public class UpdateParams
    {
      public long Id { get; set; }

      public string Key { get; set; }

      public string Name { get; set; }

      public string Url { get; set; }
    }

    [HttpPost("/midos/apps/update")]
    public INotifyResult<IMessageObject> Update([FromBody] UpdateParams param)
    {
      var app = _domain.Set<App>().Find(param.Id);

      app.Update(
        key: param.Key,
        name: param.Name,
        url: param.Url
      );
      _domain.SaveChanges();

      return NotifyResult
        .FromVoid()
        .Success("应用配置已更新");
    }

    public class AppDeleteParams
    {
      public long Id { get; set; }
    }

    [HttpPost("/midos/apps/delete")]
    public INotifyResult<IMessageObject> Update([FromBody] AppDeleteParams param)
    {
      var app = _domain.Set<App>().Find(param.Id);

      _domain.Remove(app);
      _domain.SaveChanges();

      return NotifyResult
        .FromVoid()
        .Success("应用已删除");
    }

    public class AppSearchParams
    {
      public string Class { get; set; } = "default";
    }

    [HttpPost("/midos/apps/search")]
    public IResult<App[]> All([FromBody] AppSearchParams param)
    {
      return Result.From(_domain.Set<App>()
        .Where(app => app.Class == param.Class)
        .ToArray()
      );
    }
  }
}
