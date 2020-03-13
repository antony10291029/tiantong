using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class WarehouseRepository : Repository<Warehouse, int>
  {
    public WarehouseRepository(DbContext db) : base(db)
    {

    }

    public Warehouse[] All()
    {
      return Table.OrderBy(wh => wh.id).ToArray();
    }

    public Warehouse[] Search(int userId)
    {
      return Table
        .Where(wh => wh.owner_user_id == userId)
        .OrderBy(wh => wh.number)
        .OrderBy(wh => wh.id)
        .ToArray();
    }

    public bool HasId(int id)
    {
      return Table.Any(wh => wh.id == id);
    }

    public bool HasOwner(int id, int userId)
    {
      return Table.Any(wh => wh.id == id && wh.owner_user_id == userId);
    }

    public Warehouse EnsureGet(int id)
    {
      var warehouse = Get(id);

      if (warehouse == null) {
        throw new HttpException("warehouse id does not exist");
      }

      return warehouse;
    }

    public Warehouse EnsureGetByOwner(int warehouseId, int userId)
    {
      var warehouse = EnsureGet(warehouseId);

      if (warehouse.owner_user_id != userId) {
        throw new HttpException("warehouse owner is invalid");
      }

      return warehouse;
    }

    public void EnsureOwner(int warehouseId, int userId)
    {
      if (!HasOwner(warehouseId, userId)) {
        throw new HttpException("Warehouse owner check failed");
      }
    }

  }
}
