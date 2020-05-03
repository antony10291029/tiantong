using DBCore;

namespace Wcs.Plc.DB.Sqlite
{
  public class CreatePlcConnectionsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.CreatePlcConnectionsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("DELETE TABLE IF NOT EXISTS plc_connections");
    }
  }
}
