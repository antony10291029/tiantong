using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateOrderItemProjectsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0043_CreateOrderItemProjectsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists order_item_projects");
    }
  }
}
