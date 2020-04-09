using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreatePurchaseOrderItemFinancesTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0043_CreatePurchaseOrderItemFinancesTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists purchase_order_item_finances");
    }
  }
}
