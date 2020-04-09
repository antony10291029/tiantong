using System.Linq;
using Renet.Web;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Wms.Api
{
  public class PurchaseOrderRepository : Repository<PurchaseOrder, int>
  {
    public PurchaseOrderRepository(DbContext db) : base(db)
    {

    }

    public void Ensure(int warehouseId, int id)
    {
      if (!Table.Any(order => order.warehouse_id == warehouseId && order.id == id)) {
        throw new FailureOperation("订单不存在");
      }
    }

    public override PurchaseOrder Add(PurchaseOrder order)
    {
      DbContext.Add(order);

      return order;
    }

    public override PurchaseOrder Update(PurchaseOrder order)
    {
      var currentOrder = EnsureGet(order.warehouse_id, order.id);

      foreach (var item in currentOrder.items) {
        if (!order.items.Any(i => i.id == item.id)) {
          DbContext.Remove(item);
          DbContext.RemoveRange(item.projects);
        }
      }

      foreach (var item in order.items) {
        var currentItem = currentOrder.items.FirstOrDefault(i => i.id == item.id);

        if (currentItem == null || item.id == 0) {
          currentOrder.items.Add(item);
        } else {
          foreach (var project in currentItem.projects) {
            if (!item.projects.Any(p => p.id == project.id)) {
              DbContext.Remove(project);
            }
          }
          foreach (var project in item.projects) {
            var currentProject = currentItem.projects.FirstOrDefault(p => p.id == project.id);

            if (currentProject == null || project.id == 0) {
              currentItem.projects.Add(project);
            } else {
              DbContext.Entry(currentProject).CurrentValues.SetValues(project);
            }
          }

          DbContext.Entry(currentItem).CurrentValues.SetValues(item);
          DbContext.Entry(currentItem.finance).CurrentValues.SetValues(item.finance);
        }
        
      }

      DbContext.Entry(currentOrder).CurrentValues.SetValues(order);

      return order;
    }

    public PurchaseOrder EnsureGet(int warehouseId, int id)
    {
      var order = Table
        .Include(o => o.items)
          .ThenInclude(o => o.finance)
        .Include(o => o.items)
          .ThenInclude(o => o.projects)
        .Include(o => o.payments)
        .Where(o => o.warehouse_id == warehouseId && o.id == id)
        .SingleOrDefault();

      if (order == null) {
        throw new FailureOperation("订单不存在");
      }

      return order;
    }

    public bool HasNumber(int warehouseId, string number)
    {
      return Table.Any(order => order.warehouse_id == warehouseId && order.number == number);
    }

    public void EnsureNumberUnique(int warehouseId, string number)
    {
      if (HasNumber(warehouseId, number)) {
        throw new HttpException("Order number already exists in this warehouse");
      }
    }

  }
}
