using DBCore;
using Microsoft.AspNetCore.Mvc;
using Midos.Center.Aggregates;
using Midos.Center.Entities;
using Midos.Domain;
using System;
using System.Linq;

namespace Midos.Center.Controllers
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
      SeedConfigs();
      SeedApps();
      SeedTaskTypes();
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

    private void SeedTaskTypes()
    {
      var types = Enumerable.Range(1, 10)
        .Select(key => TaskType.From(
          key: $"type_key_{key}",
          name: $"type_name_{key}",
          hasCode: false,
          data: null,
          comment: $"type_comment_{key}",
          subtypes: Enumerable.Range(1, 5)
            .Select(i => SubtaskType.From(
              key: $"subtype_key_{key}_{i}",
              index: i,
              subtype: TaskType.From(
                key: $"subtype_key_{key}_{i}",
                name: $"subtype_name_{i}",
                hasCode: false,
                comment: $"subtype_comment_{i}",
                data: null
              )
            )).ToList()
        )).ToArray();

      _domain.AddRange(types);
      _domain.SaveChanges();
    }
  }
}
