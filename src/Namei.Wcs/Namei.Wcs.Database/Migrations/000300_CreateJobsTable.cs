using DBCore;

namespace Namei.Wcs.Database
{
  public class CreateJobsTable: IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000300_CreateJobsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists jobs");
    }
  }
}
