using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateProjectsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0003_CreateProjectsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteFromSql("Migration.0003_DropProjectsTable");
    }
  }
}
