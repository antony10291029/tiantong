using DBCore;

namespace Tiantong.Iot.Sqlite.Log
{
  public class CreateHttpPusherErrorsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.006000_CreateHttpPusherErrorsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists http_pusher_errors");
    }

  }

}
