using DBCore;

namespace Midos.Center.Database
{
  public class CreateTaskOrdersTable: IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000400_CreateTaskOrdersTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists task_orders");
    }
  }
}
