using DBCore;

namespace Tiantong.Iot.DB.Sqlite
{
  public class CreatePlcConnectionLogsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.CreatePlcConnectionLogsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("DELETE TABLE IF NOT EXISTS plc_connection_logs");
    }
  }
}
