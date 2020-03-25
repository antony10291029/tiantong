using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateSuppliersTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0025_CreateSuppliersTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists suppliers");
    }
  }
}
