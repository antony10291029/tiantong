using DBCore;

namespace Tiantong.Iot.Sqlite.Log
{
  public class CreatePlcStateLogsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.003000_CreatePlcStateLogsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists plc_state_logs");
    }

  }

}
