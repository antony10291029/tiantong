using Midos.Center.Database;

namespace Midos.Center
{
  public class DomainMigrator: PostgresMigrator
  {
    public DomainMigrator(DomainContext domain): base(domain)
    {

    }
  }
}