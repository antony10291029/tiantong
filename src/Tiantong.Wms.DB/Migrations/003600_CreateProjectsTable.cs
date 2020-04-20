using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateProjectsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.003600_CreateProjectsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists projects");
    }
  }
}
