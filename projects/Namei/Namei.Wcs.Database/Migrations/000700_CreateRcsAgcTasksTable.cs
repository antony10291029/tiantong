using DBCore;

namespace Namei.Wcs.Database
{
  public class CreateRcsAgcTasksTable: IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000700_CreateRcsAgcTasksTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists \"RcsAgcTasks\"");
    }
  }
}
