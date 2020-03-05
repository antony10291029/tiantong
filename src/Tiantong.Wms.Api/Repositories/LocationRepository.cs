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

    public void EnsureNumberUnique(int areaId, string number)
    {
      if (Table.Any(item => item.area_id == areaId && item.number == number)) {
        throw new HttpException("Location number already exists in this warehouse");
      }
    }
  }
}
