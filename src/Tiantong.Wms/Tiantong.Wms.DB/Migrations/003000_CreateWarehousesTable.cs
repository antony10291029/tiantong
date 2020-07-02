using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateWarehousesTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.003000_CreateWarehousesTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists warehouses");
    }
  }
}