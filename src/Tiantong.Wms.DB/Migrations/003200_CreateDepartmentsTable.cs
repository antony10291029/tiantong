using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateDepartmentsTable : IMigration
  {
    public void Up(DBCore.DbContext db)
    {
      db.ExecuteFromSql("Migration.003200_CreateDepartmentsTable");
    }

    public void Down(DBCore.DbContext db)
    {
      db.ExecuteSql("drop table if exists departments");
    }
  }
}
