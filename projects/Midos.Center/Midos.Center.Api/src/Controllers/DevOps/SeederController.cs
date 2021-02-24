using System;
using DBCore;
using Microsoft.AspNetCore.Mvc;
using Midos.Center.Entities;

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
    }

    protected override void Reseed()
    {
      _migrator.Refresh();
      Seed();
    }

    private void SeedConfigs()
    {
      _domain.Configs.AddRange(new Config[] {
        new Config { Key = "midos.key1", Value = "midos.value1", UpdatedAt = DateTime.Now },
        new Config { Key = "midos.key2", Value = "midos.value2", UpdatedAt = DateTime.Now },
      });

      _domain.SaveChanges();
    }
  }
}
