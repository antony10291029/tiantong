using DBCore;

namespace Tiantong.Iot.DB.Sqlite
{
  public class CreateHttpPusherErrorsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.003200_CreateHttpPusherErrorsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists http_pusher_errors");
    }
  }
}
