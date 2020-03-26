using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateKeepersTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0022_CreateKeepersTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists keepers");
    }
  }
}
