using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateItemsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0040_CreateItemsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists items");
    }
  }
}
