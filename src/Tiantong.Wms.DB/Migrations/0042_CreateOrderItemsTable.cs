using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateOrderItemsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0042_CreateOrderItemsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists order_items");
    }
  }
}
