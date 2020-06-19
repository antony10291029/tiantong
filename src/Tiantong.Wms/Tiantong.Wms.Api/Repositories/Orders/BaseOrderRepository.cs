using System.Data;
using System.Linq;
using Renet.Web;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Wms.Api
{
  public class BaseOrderRepository : Repository<Order, int>
  {
    public BaseOrderRepository(DbContext db) : base(db)
    {

    }

    public bool HasProject(int projectId)
    {
      return DbContext.OrderItemProjects.Any(p => p.project_id == projectId);
    }

    public bool HasItem(int itemId)
    {
      return DbContext.OrderItems.Any(p => p.item_id == itemId);
    }

    public bool HasItem(int[] itemIds)
    {
      return DbContext.OrderItems.Any(p => itemIds.Contains(p.item_id));
    }

    public bool HasGood(int goodId)
    {
      return DbContext.OrderItems.Any(p => p.good_id == goodId);
    }

    public bool HasWarehouse(int warehouseId)
    {
      return Table.Any(o => o.warehouse_id == warehouseId);
    }

    public bool HasDepartment(int departmentId)
    {
      return Table.Any(o => o.department_id == departmentId);
    }

    public bool HasSupplier(int supplierId)
    {
      return Table.Any(o => o.supplier_id == supplierId);
    }

    public bool HasUser(int userId)
    {
      return Table.Any(o => o.applicant_id == userId || o.operator_id == userId);
    }

    public bool HasNumber(int warehouseId, string number)
    {
      return Table.Any(order => order.warehouse_id == warehouseId && order.number == number);
    }

  }
}
