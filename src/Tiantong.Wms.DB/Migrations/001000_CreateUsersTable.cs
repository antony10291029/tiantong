using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateUsersTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.001000_CreateUsersTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists users");
    }
  }
}
