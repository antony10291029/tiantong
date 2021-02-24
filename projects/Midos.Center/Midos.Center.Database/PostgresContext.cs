namespace Midos.Center.Database
{
  public class PostgresContext: DBCore.DbContext
  {
    public PostgresContext()
    {
      UseAssembly(typeof(PostgresContext).Assembly);
    }
  }
}
