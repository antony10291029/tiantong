using DBCore;

namespace Tiantong.Iot.Sqlite.Log
{
  public class CreatePlcStateErrorsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.004000_CreatePlcStateErrorsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists plc_state_errors");
    }

  }

}
