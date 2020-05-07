using DBCore;

namespace Tiantong.Iot.DB.Sqlite
{
  public class CreatePlcStateHttpPushersTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.002300_CreatePlcStateHttpPushersTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists plc_state_http_pushers");
    }
  }
}
