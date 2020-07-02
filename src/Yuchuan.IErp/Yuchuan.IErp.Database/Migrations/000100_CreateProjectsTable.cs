using DBCore;

namespace Yuchuan.IErp.Database
{
  public class CreateProjectsTable: IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000100_CreateProjectsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists project_devices");
      db.ExecuteSql("drop table if exists project_users");
      db.ExecuteSql("drop table if exists projects");
    }
  }
}
