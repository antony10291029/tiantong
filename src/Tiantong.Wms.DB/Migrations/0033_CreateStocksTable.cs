using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreateStocksTable : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0033_CreateStocksTable");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists stocks");
    }
  }
}
