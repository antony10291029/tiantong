using System.Data;
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

    public override PurchaseOrder Add(PurchaseOrder order)
    {
      order.status = PurchaseOrderStatuses.Created;
      DbContext.Add(order);

      return order;
    }

    public override PurchaseOrder Update(PurchaseOrder order)
    {
      var oldOrder = EnsureGet(order.warehouse_id, order.id);

      if (oldOrder.status != PurchaseOrderStatuses.Created) {
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
            DbContext.Entry(oldItem.finance).CurrentValues.SetValues(item.finance);
          }
        }
      }

      DbContext.Entry(oldOrder).CurrentValues.SetValues(order);
      DbContext.Entry(oldOrder).Property(o => o.status).IsModified = false;

      return order;
    }

    public PurchaseOrder EnsureGet(int warehouseId, int id)
    {
      var order = Table
        .Include(o => o.items)
          .ThenInclude(oi => oi.finance)
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

    public bool HasProject(int projectId)
    {
      return DbContext.PurchaseItemProjects.Any(p => p.project_id == projectId);
    }

    public bool HasItem(int itemId)
    {
      return DbContext.PurchaseOrderItems.Any(p => p.item_id == itemId);
    }

    public bool HasItem(int[] ids)
    {
      return DbContext.PurchaseOrderItems.Any(p => ids.Contains(p.item_id));
    }

    public bool HasGood(int goodId)
    {
      return DbContext.PurchaseOrderItems.Any(p => p.good_id == goodId);
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
