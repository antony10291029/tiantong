using DBCore;

namespace Tiantong.Iot.DB.Sqlite
{
  public class CreatePlcStateLogsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.002100_CreatePlcStateLogsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("delete table if exists plc_state_logs");
    }
  }
}
