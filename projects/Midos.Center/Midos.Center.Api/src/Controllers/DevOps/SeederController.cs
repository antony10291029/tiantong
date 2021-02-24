using Microsoft.AspNetCore.Mvc;
using DBCore;
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

    }

    protected override void Reseed()
    {
      _migrator.Refresh();
      Seed();
    }
  }
}
