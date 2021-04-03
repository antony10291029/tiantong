using System.Linq;
using System;
using DBCore;
using Microsoft.AspNetCore.Mvc;
using Midos.Center.Entities;
using Midos.Domain;

namespace Midos.Center.Controllers
{
  public class SeederController: SeederControllerBase
  {
    private DomainContext _domain;

    private IMigrator _migrator;

    public SeederController(DomainContext domain, MigratorProvider _migratorProvider)
    {
      _domain = domain;
      _migrator = _migratorProvider.Migrator;
    }

    protected override void Seed()
    {
      SeedConfigs();
      SeedApps();
      SeedTaskTypes();
    }

    protected override void Reseed()
    {
      _migrator.Refresh();
      Seed();
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
          comment: $"type_comment_{key}"
        )).ToArray();

      var subtypes = Enumerable.Range(1, 20)
        .Select(key => TaskType.From(
          key: $"subtype_key_{key}",
          name: $"subtype_name_{key}",
          hasCode: false,
          data: null,
          comment: $"subtype_comment_{key}"
        )).ToArray();

      _domain.AddRange(types);
      _domain.AddRange(subtypes);
      _domain.SaveChanges();

      var relations = Enumerable.Range(1, 5)
        .Select(index => SubtaskType.From(
          key: $"test_subkey_{index}",
          index: index,
          typeId: types[index].Id,
          subtypeId: subtypes[index].Id
        ));
      
      _domain.AddRange(relations);
      _domain.SaveChanges();
    }
  }
}
