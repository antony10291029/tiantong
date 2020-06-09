using System.Reflection;

namespace DBCore.Sqlite
{
  public class Migrator : DBCore.Migrator
  {
    public Migrator(DbContext db): base(db)
    {

    }

    protected override void Initialize()
    {
      var dir = DbContext.SqlDirectory();
      DbContext.UseSqlDirectory("Sql");
      DbContext.UseAssembly(typeof(Migrator).Assembly);
      UseAssembly(Assembly.GetExecutingAssembly());
      DbContext.ExecuteFromSql("CreateMigrationsTable");
      UseAssembly(GetType().Assembly);
      DbContext.UseAssembly(GetType().Assembly);
      DbContext.UseSqlDirectory(dir);
    }

    protected override bool IsInitialized()
    {
      return false;
    }
  }
}