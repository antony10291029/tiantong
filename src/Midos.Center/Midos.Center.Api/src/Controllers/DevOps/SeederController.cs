using DBCore;
using Microsoft.AspNetCore.Mvc;
using Midos.Center.Entities;
using Midos.Domain;
using System;
using System.Linq;

namespace Midos.Center.Controllers
{
  public class SeederController: SeederControllerBase
  {
    private readonly DomainContext _domain;

    public SeederController(DomainContext domain, IMigrator migrator): base(migrator)
    {
      _domain = domain;
    }

    protected override void Seed()
    {
      SeedConfigs();
      SeedApps();
    }

    private void SeedConfigs()
    {
      _domain.Set<Config>().AddRange(new Config[] {
        new Config { Key = "midos.key1", Value = "midos.value1", UpdatedAt = DateTime.Now },
        new Config { Key = "midos.key2", Value = "midos.value2", UpdatedAt = DateTime.Now },
      });

      _domain.SaveChanges();
    }

    private void SeedApps()
    {
      _domain.Set<App>().AddRange(
        Enumerable.Range(1, 10)
          .Select(index => App.From(
            klass: "default",
            key: $"test_app_{index}",
            name: $"测试应用_{index}",
            url: $"http://localhost:800{index}"
          ))
          .ToArray()
      );
      _domain.SaveChanges();
    }
  }
}
