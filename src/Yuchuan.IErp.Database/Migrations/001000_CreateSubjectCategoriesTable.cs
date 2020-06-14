using DBCore;

namespace Yuchuan.IErp.Database
{
  public class CreateSubjectCategoriesTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("001000_CreateSubjectCategoriesTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists subject_categories");
    }
  }
}
