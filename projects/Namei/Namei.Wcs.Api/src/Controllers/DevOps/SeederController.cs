using System.Linq;
using DBCore;
using Microsoft.AspNetCore.Mvc;
using Namei.Wcs.Aggregates;
using System.Collections.Generic;

namespace Namei.Wcs.Api
{
  public class SeederController: SeederControllerBase
  {
    private DomainContext _domain;

    public SeederController(DomainContext domain, IMigrator migrator): base(migrator)
    {
      _domain = domain;
    }

    protected override void Seed()
    {
      SeedRcsAgcTasks();
    }

    private void SeedRcsAgcTasks()
    {
      foreach (var i in Enumerable.Range(1, 1000)) {
        _domain.Add(RcsAgcTask.From(
          taskType: "wcs.put",
          position: (1000 + i).ToString(),
          destination: (8000 + i).ToString(),
          podCode: (10000 + i).ToString(),
          comment: $"测试任务 {i}",
          orderType: "asdfasdf",
          orderId: 10000
        ));
      }

      _domain.SaveChanges();
    }
  }
}
