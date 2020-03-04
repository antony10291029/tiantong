using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateAreasTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0022_CreateAreasTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists areas");
    }
  }
}
