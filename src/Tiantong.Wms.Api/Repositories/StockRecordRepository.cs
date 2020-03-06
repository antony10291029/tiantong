namespace Tiantong.Wms.Api
{
  public class StockRecordRepository : Repository<StockRecord, int>
  {
    public StockRecordRepository(DbContext db) : base(db)
    {

    }
  }
}
