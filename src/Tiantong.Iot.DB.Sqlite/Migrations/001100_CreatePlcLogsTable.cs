using DBCore;

namespace Tiantong.Iot.DB.Sqlite
{
  public class CreatePlcLogsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.001100_CreatePlcLogsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists plc_logs");
    }
  }
}
