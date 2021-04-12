namespace DBCore.Postgres
{
  public class Migrator : DBCore.Migrator
  {
    public Migrator(DbContext db): base(db)
    {

    }

    protected override void Initialize()
    {
      DbContext.ExecuteFromSql(
        name: "CreateMigrationsTable",
        assembly: typeof(Migrator).Assembly
      );
    }
  }
}
