using DBCore;

namespace Yuchuan.IErp.Database
{
  public class CreateSubjectSubCategoriesTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("002000_CreateSubjectSubCategoriesTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists subject_sub_categories");
    }
  }
}
