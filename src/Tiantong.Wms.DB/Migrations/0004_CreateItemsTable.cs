using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateItemsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0004_CreateItemsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteFromSql("Migration.0004_DropItemsTable");
    }
  }
}
