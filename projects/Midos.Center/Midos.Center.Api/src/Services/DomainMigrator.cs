using Midos.Center.Database;
using Midos.Domain;

namespace Midos.Center
{
  public class DomainMigrator: PostgresMigrator
  {
    public DomainMigrator(DomainContext domain): base(domain)
    {

    }
  }
}