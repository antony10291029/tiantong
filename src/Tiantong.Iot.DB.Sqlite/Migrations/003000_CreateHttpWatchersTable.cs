using DBCore;

namespace Tiantong.Iot.DB.Sqlite
{
  public class CreateHttpWatchersTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.003000_CreateHttpWatchersTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("delete table if exists http_watchers");
    }
  }
}
