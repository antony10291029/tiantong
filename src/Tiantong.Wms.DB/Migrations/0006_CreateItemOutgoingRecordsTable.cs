using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateItemOutgoingRecordsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0006_CreateItemOutgoingRecordsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteFromSql("Migration.0006_DropItemOutgoingRecordsTable");
    }
  }
}
