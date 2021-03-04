using DBCore;

namespace Namei.Wcs.Database
{
  public class CreateLifterRuntimeTasksTable: IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000410_CreateLifterRuntimeTasksTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists lifter_runtime_tasks");
    }
  }
}
