using DBCore;

namespace Tiantong.Account.Database
{
  public class CreateUsersTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000100_CreateUsersTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists users");
    }
  }
}
