using DBCore;

namespace Tiantong.Iot.DB.Sqlite
{
  public class CreateHttpWatcherErrorsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.003200_CreateHttpWatcherErrorsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("delete table if exists http_watcher_errors");
    }
  }
}
