using DBCore;

namespace Midos.Center.Database
{
  public class CreateAppsTable: IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000200_CreateAppsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists apps");
    }
  }
}
