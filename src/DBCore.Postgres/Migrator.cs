namespace DBCore.Postgres
{
  public class Migrator : DBCore.Migrator
  {
    public Migrator(DbContext db): base(db)
    {

    }
    
    protected override void Initialize()
    {
      DbContext.ExecuteFromSql("CreateMigrationsTable");
    }

    protected override bool IsInitialized()
    {
      return false;
    }
  }
}
