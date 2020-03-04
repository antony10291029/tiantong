using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateStockRecordsTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0042_CreateStockRecordsTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists stock_records");
    }
  }
}
