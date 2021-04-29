using DBCore;

namespace Namei.Wcs.Database
{
  public class AgcTaskTypes: IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000710_AgcTaskTypes");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists \"AgcTaskTypes\"");
    }
  }
}
