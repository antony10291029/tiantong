using DBCore;

namespace Midos.Center.Database
{
  public class CreateSubtaskTypesTable: IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("000310_CreateSubtaskTypesTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists \"SubtaskTypes\"");
    }
  }
}
