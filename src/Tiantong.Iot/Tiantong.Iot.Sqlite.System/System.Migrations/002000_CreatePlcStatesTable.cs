using DBCore;

namespace Tiantong.Iot.Sqlite.System
{
  public class CreatePlcStatesTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("002000_CreatePlcStatesTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists plc_states");
    }

  }

}
