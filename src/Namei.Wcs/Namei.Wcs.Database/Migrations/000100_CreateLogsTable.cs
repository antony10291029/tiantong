using DBCore;

namespace Namei.Wcs.Database
{
  public class CreateLogsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000100_CreateLogsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists logs");
    }
  }
}
