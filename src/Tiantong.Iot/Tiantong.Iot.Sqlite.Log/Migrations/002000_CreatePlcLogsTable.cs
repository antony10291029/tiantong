using DBCore;

namespace Tiantong.Iot.Sqlite.Log
{
  public class CreatePlcLogsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.002000_CreatePlcLogsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists plc_logs");
    }

  }

}
