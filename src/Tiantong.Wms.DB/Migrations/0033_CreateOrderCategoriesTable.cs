using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateOrderCategoriesTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0033_CreateOrderCategoriesTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists order_categories");
    }
  }
}