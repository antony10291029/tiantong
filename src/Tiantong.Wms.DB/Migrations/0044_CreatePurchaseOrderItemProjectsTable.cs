using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreatePurchaseOrderItemProjectsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0044_CreatePurchaseOrderItemProjectsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists purchase_order_item_projects");
    }
  }
}
