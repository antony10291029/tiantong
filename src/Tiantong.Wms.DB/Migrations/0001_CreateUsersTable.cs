using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateUsersTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0001_CreateUsersTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteFromSql("Migration.0001_DropUsersTable");
    }
  }
}
