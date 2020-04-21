using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class AreaRepository : Repository<Area, int>
  {
    private WarehouseRepository _warehouses;

    public AreaRepository(DbContext db, WarehouseRepository warehouses) : base(db)
    {
      _warehouses = warehouses;
    }

    //

    public bool HasId(int warehouseId, int id)
    {
      return Table.Any(area => area.warehouse_id == warehouseId && area.id == id);
    }

    public bool HasNumber(int warehouseId, string number)
    {
      return Table.Any(area => area.warehouse_id == warehouseId && area.number == number);
    }

    public Area EnsureGet(int id)
    {
      var area = Get(id);

      if (area == null) {
        throw new HttpException("area id does not exist");
      }

      return area;
    }

    public Area EnsureGetByOwner(int id, int userId)
    {
      var area = EnsureGet(id);
      _warehouses.EnsureUser(area.warehouse_id, userId);

      return area;
    }

    public void EnsureNumberUnique(int warehouseId, string number)
    {
      if (HasNumber(warehouseId, number)) {
        throw new HttpException("area number already exists in this warehouse");
      }
    }

    public Area[] Search(int warehouseId)
    {
      return Table.Where(area => area.warehouse_id == warehouseId)
        .OrderBy(area => area.id).ToArray();
    }
  }
}
