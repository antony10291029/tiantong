using DBCore;

namespace Namei.Wcs.Database
{
  public class CreateRcsDoorTasksTable: IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000500_CreateRcsDoorTasksTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists \"RcsDoorTasks\"");
    }
  }
}
