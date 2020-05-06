using DBCore;

namespace Tiantong.Iot.DB.Sqlite
{
  public class CreatePlcErrorsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.001200_CreatePlcErrorsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists plc_errors");
    }
  }
}
