using DBCore;

namespace Yuchuan.IErp.Database
{
  public class CreateUsersTable: IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("002000_CreateUsersTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists users");
    }
  }
}
