using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateProjectItemsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0052_CreateProjectItemsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists project_items");
    }
  }
}
