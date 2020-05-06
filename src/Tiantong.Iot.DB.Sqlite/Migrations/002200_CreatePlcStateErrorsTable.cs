using DBCore;

namespace Tiantong.Iot.DB.Sqlite
{
  public class CreatePlcStateErrorsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.002200_CreatePlcStateErrorsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists plc_state_errors");
    }
  }
}
