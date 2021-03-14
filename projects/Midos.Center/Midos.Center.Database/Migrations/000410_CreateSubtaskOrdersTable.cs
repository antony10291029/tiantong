using DBCore;

namespace Midos.Center.Database
{
  public class CreateSubtaskOrdersTable: IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000410_CreateSubtaskOrdersTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists subtask_orders");
    }
  }
}
