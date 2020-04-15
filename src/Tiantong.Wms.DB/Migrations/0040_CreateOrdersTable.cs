using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateOrdersTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0040_CreateOrdersTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists orders");
    }
  }
}
