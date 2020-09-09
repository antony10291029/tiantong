using DBCore;

namespace Namei.Wcs.Database
{
  public class CreateLifterTasksTable: IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000400_CreateLifterTasksTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists lifter_tasks");
    }
  }
}
