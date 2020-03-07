using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateOrderProjectsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0052_CreateOrderProjectsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists order_project_items");
    }
  }
}
