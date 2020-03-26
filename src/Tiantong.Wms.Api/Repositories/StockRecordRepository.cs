namespace Tiantong.Wms.Api
{
  public class PurchaseItemProjectRepository : Repository<PurchaseItemProject>
  {
    public PurchaseItemProjectRepository(DbContext db) : base(db)
    {

    }
  }
}
