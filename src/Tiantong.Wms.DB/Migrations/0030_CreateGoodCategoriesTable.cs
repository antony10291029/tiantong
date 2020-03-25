using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateGoodCategoriesTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0030_CreateGoodCategoriesTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists good_categories");
    }
  }
}
