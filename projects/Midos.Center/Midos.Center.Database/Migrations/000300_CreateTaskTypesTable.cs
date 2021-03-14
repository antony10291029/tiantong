using DBCore;

namespace Midos.Center.Database
{
  public class CreateTaskTypesTable: IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000300_CreateTaskTypesTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists task_types");
    }
  }
}
