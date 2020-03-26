using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateLocationsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0024_CreateLocationsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists locations");
    }
  }
}
