using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateWarehouseKeepersTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0021_CreateWarehouseKeepersTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists warehouse_keepers");
    }
  }
}
