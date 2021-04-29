using DBCore;

namespace Namei.Wcs.Database
{
  public class AgcTasks: IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000700_AgcTasks");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists \"AgcTasks\"");
    }
  }
}
