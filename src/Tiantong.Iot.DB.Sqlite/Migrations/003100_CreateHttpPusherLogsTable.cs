using DBCore;

namespace Tiantong.Iot.DB.Sqlite
{
  public class CreateHttpPusherLogsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.003100_CreateHttpPusherLogsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists http_pusher_logs");
    }
  }
}
