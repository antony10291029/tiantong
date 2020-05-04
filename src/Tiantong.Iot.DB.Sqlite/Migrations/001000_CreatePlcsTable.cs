using DBCore;

namespace Tiantong.Iot.DB.Sqlite
{
  public class CreatePlcsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.001000_CreatePlcsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("delete table if exists plcs");
    }
  }
}
