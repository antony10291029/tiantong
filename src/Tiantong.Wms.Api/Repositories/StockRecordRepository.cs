namespace Tiantong.Wms.Api
{
  public class StockRecordRepository : Repository<StockRecord>
  {
    public StockRecordRepository(DbContext db) : base(db)
    {

    }
  }
}
