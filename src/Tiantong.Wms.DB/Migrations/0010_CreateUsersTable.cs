using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateUsersTable : IMigration
  {
    public void Up(DBCore.DbContext db)
    {
      db.ExecuteFromSql("Migration.0010_CreateUsersTable");
    }

    public void Down(DBCore.DbContext db)
    {
      db.ExecuteSql("drop table if exists users");
    }
  }
}
