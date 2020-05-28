using DBCore;

namespace Tiantong.Iot.Sqlite.System
{
  public class CreatePlcsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.001000_CreatePlcsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists plcs");
    }

  }

}
