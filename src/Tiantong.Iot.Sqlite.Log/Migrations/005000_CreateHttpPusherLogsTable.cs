using DBCore;

namespace Tiantong.Iot.Sqlite.Log
{
  public class CreateHttpPusherLogsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.005000_CreateHttpPusherLogsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists http_pusher_logs");
    }

  }

}
