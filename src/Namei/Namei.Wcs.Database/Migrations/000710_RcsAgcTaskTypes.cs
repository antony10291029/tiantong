using DBCore;

namespace Namei.Wcs.Database
{
  public class RcsAgcTaskTypes: IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000710_RcsAgcTaskTypes");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists \"RcsAgcTaskTypes\"");
    }
  }
}
