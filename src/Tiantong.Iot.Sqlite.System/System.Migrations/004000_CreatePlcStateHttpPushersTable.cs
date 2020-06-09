using DBCore;

namespace Tiantong.Iot.Sqlite.System
{
  public class CreatePlcStateHttpPushersTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("004000_CreatePlcStateHttpPushersTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists plc_state_http_pushers");
    }

  }

}
