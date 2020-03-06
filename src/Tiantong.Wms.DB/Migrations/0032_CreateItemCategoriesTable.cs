using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateItemCategoriesTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0032_CreateItemCategoriesTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists item_categories");
    }
  }
}
