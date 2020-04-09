
using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreatePurchasePaymentsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0041_CreatePurchasePaymentsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists purchase_payments");
    }
  }
}
