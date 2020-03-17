using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class LocationRepository : Repository<Location, int>
  {
    private WarehouseRepository _warehouses;

    public LocationRepository(DbContext db, WarehouseRepository warehouses) : base(db)
    {
      _warehouses = warehouses;
    }

    //

    public Location[] Search(int warehouseId)
    {
      return Table.Where(location => location.warehouse_id == warehouseId)
        .OrderBy(location => location.number).ToArray();
    }

    public Location EnsureGet(int id)
    {
      var location = Get(id);

      if (location == null) {
        throw new HttpException("Location id does not exist");
      }

      return location;
    }

    public Location EnsureGetByOwner(int id, int userId)
    {
      var location = EnsureGet(id);
      _warehouses.EnsureOwner(location.warehouse_id, userId);

      return location;
    }

    public void EnsureIds(int warehouseId, int[] ids)
    {
      var count = Table.Where(location =>
        location.warehouse_id == warehouseId &&
        ids.Contains(location.id)
      ).Count();

      if (count != ids.Length) {
        throw new HttpException("Location ids do not exists in this warehouse");
      }
    }

    public void EnsureNumberUnique(int warehouseId, string number)
    {
      if (Table.Any(item => item.warehouse_id == warehouseId && item.number == number)) {
        throw new HttpException("Location number already exists in this warehouse");
      }
    }
  }
}
