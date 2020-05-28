using DBCore;

namespace Tiantong.Iot.Sqlite.System
{
  public class CreateHttpPushersTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.003000_CreateHttpPushersTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists http_pushers");
    }

  }

}
