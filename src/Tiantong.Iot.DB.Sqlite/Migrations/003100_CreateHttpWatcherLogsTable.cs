using DBCore;

namespace Tiantong.Iot.DB.Sqlite
{
  public class CreateHttpWatcherLogsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.003100_CreateHttpWatcherLogsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("delete table if exists http_watcher_logs");
    }
  }
}
