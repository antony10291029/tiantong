using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Wms.Api
{
  public class WarehouseRepository : Repository<Warehouse, int>
  {
    protected DbSet<Warehouse> Warehouses { get => DbContext.Warehouses; }

    public WarehouseRepository(DbContext db) : base(db)
    {

    }

    public bool HasId(int id)
    {
      return Warehouses.Any(wh => wh.id == id);
    }

    public bool HasOwner(int id, int userId)
    {
      return Warehouses.Any(wh => wh.id == id && wh.owner_user_id == userId);
    }

    public bool HasNumber(int userId, string number)
    {
      return Warehouses.Any(wh => wh.owner_user_id == userId && wh.number == number);
    }

    public Warehouse[] Search(int userId)
    {
      return Warehouses
        .Where(wh => wh.owner_user_id == userId)
        .OrderBy(wh => wh.id)
        .ToArray();
    }
  }
}
