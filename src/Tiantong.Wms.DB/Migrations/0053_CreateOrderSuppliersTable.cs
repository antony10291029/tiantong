using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateOrderSuppliersTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0053_CreateOrderSuppliersTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists order_suppliers");
    }
  }
}
