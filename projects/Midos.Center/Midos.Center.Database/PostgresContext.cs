using Midos.Domain;

namespace Midos.Center.Database
{
  public class PostgresContext: DomainContext
  {
    public PostgresContext()
    {
      UseAssembly(typeof(PostgresContext).Assembly);
    }
  }
}
