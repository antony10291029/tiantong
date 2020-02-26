using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateItemStorageRecordsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0005_CreateItemStorageRecordsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteFromSql("Migration.0005_DropItemStorageRecordsTable");
    }
  }
}
