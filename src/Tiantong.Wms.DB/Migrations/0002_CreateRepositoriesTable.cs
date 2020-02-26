using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateRepositoriesTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0002_CreateRepositoriesTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteFromSql("Migration.0002_DropRepositoriesTable");
    }
  }
}
