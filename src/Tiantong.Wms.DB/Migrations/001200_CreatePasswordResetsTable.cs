using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreatePasswordResetsTable : IMigration
  {
    public void Up(DBCore.DbContext db)
    {
      db.ExecuteFromSql("Migration.001200_CreatePasswordResetsTable");
    }

    public void Down(DBCore.DbContext db)
    {
      db.ExecuteSql("drop table if exists password_resets");
    }
  }
}
