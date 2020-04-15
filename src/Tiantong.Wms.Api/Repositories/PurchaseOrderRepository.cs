using System.Data;
using System.Linq;
using Renet.Web;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Wms.Api
{
  public class PurchaseOrderRepository : OrderRepository
  {
    public PurchaseOrderRepository(DbContext db) : base(db)
    {

    }

    public override Order Add(Order order)
    {
      order.status = OrderStatus.Created;
      DbContext.Add(order);

      return order;
    }

    public override Order Update(Order order)
    {
      var oldOrder = EnsureGet(order.warehouse_id, order.id);

      if (oldOrder.status != OrderStatus.Created) {
        throw new FailureOperation("订单已入库，无法再次修改");
      }

      foreach (var payment in oldOrder.payments) {
        if (!order.payments.Any(p => p.id == payment.id)) {
          DbContext.Remove(payment);
        }
      }

      foreach (var payment in order.payments) {
        if (payment.id == 0) {
          oldOrder.payments.Add(payment);
        } else {
          var oldPayment = oldOrder.payments.FirstOrDefault(p => p.id == payment.id);

          if (oldPayment != null) {
            DbContext.Entry(oldPayment).CurrentValues.SetValues(payment);
          }
        }
      }

      foreach (var item in oldOrder.items) {
        if (!order.items.Any(i => i.id == item.id)) {
          DbContext.Remove(item);
          DbContext.RemoveRange(item.projects);
        }
      }

      foreach (var item in order.items) {
        if (item.id == 0) {
          oldOrder.items.Add(item);
        } else {
          var oldItem = oldOrder.items.FirstOrDefault(i => i.id == item.id);

          if (oldItem != null) {
            foreach (var project in oldItem.projects) {
              if (!item.projects.Any(p => p.id == project.id)) {
                DbContext.Remove(project);
              }
            }

            foreach (var project in item.projects) {
              if (project.id == 0) {
                oldItem.projects.Add(project);
              } else {
                var oldProject = oldItem.projects.FirstOrDefault(p => p.id == project.id);

                if (oldProject != null) {
                  DbContext.Entry(oldProject).CurrentValues.SetValues(project);
                }
              }
            }

            DbContext.Entry(oldItem).CurrentValues.SetValues(item);
            DbContext.Entry(oldItem.invoice).CurrentValues.SetValues(item.invoice);
          }
        }
      }

      DbContext.Entry(oldOrder).CurrentValues.SetValues(order);
      DbContext.Entry(oldOrder).Property(o => o.status).IsModified = false;

      return order;
    }

    public Order EnsureGet(int warehouseId, int id)
    {
      var order = Table
        .Include(o => o.items)
          .ThenInclude(oi => oi.invoice)
        .Include(o => o.items)
          .ThenInclude(oi => oi.projects)
        .Include(o => o.payments)
        .Where(o => o.warehouse_id == warehouseId && o.id == id)
        .SingleOrDefault();

      if (order == null) {
        throw new FailureOperation("订单不存在");
      }

      return order;
    }

  }
}
