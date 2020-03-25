using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreatePurchaseOrdersTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0040_CreatePurchaseOrdersTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists purchase_orders");
    }
  }
}
