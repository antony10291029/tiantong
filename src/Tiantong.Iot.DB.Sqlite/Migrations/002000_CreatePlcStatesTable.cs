using DBCore;

namespace Tiantong.Iot.DB.Sqlite
{
  public class CreatePlcStatesTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.002000_CreatePlcStatesTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("delete table if exists plc_states");
    }
  }
}
