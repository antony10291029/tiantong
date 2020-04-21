using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateDepartmentsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.003100_CreateDepartmentsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists departments");
    }
  }
}
