using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateCategoriesTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0031_CreateCategoriesTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists categories");
    }
  }
}
