using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreatePurchaseOrderItemsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0042_CreatePurchaseOrderItemsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists purchase_order_items");
    }
  }
}
