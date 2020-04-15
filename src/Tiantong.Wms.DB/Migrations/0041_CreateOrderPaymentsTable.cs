
using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateOrderPaymentsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0041_CreateOrderPaymentsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists order_payments");
    }
  }
}
