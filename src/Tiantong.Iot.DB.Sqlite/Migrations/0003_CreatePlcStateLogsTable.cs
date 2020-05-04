using DBCore;

namespace Tiantong.Iot.DB.Sqlite
{
  public class CreatePlcStateLogsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.CreatePlcStateLogsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("DELETE TABLE IF NOT EXISTS plc_state_logs");
    }
  }
}
