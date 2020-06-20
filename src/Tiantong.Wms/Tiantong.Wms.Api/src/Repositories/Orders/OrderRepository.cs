using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class OrderRepository : BaseOrderRepository
  {
    protected Auth Auth;

    protected GoodRepository Goods;

    protected StockRepository Stocks;

    protected ProjectRepository Projects;

    protected SupplierRepository Suppliers;

    protected LocationRepository Locations;

    protected WarehouseRepository Warehouses;

    protected DepartmentRepository Departments;

    public OrderRepository(
      Auth auth,
      DbContext db,
      GoodRepository goods,
      StockRepository stocks,
      ProjectRepository projects,
      LocationRepository locations,
      SupplierRepository suppliers,
      WarehouseRepository warehouses,
      DepartmentRepository departments
    ) : base(db) {
      Auth = auth;
      Goods = goods;
      Stocks = stocks;
      Projects = projects;
      Locations = locations;
      Suppliers = suppliers;
      Warehouses = warehouses;
      Departments = departments;
    }

    protected virtual bool RequireSupplier { get; } = false;

    protected virtual bool RequireDepartment { get; } = false;

    protected virtual bool RequireApplicant { get; } = false;

    private void EnsureRelationships(Order order)
    {
      Warehouses.Users.EnsureExists(order.warehouse_id, order.operator_id);
      if (RequireDepartment || (order.department_id != 0)) {
        Departments.EnsureExists(order.warehouse_id, order.department_id);
      }
      if (RequireApplicant || (order.applicant_id != 0)) {
        Warehouses.Users.EnsureExists(order.warehouse_id, order.applicant_id);
      }
      if (RequireSupplier || (order.supplier_id != 0)) {
        Suppliers.EnsureExists(order.warehouse_id, order.supplier_id);
      }

      if (order.items.Count > 0) {
        foreach (var item in order.items) {
          if (item.good_id != 0 && item.item_id != 0) {
            Goods.Items.EnsureExists(order.warehouse_id, item.good_id, item.item_id);
          }
        }
      }

      var projectIds = order.items
        .SelectMany(i => i.projects.Select(p => p.project_id))
        .Distinct().ToArray();
      if (projectIds.Length > 0) {
        Projects.EnsureExists(order.warehouse_id, projectIds);
      }
    }

    // Create

    public Order Add(Order order, string type)
    {
      order.operator_id = Auth.User.id;
      EnsureRelationships(order);
      order.type = type;
      DbContext.Add(order);

      return order;
    }

    // Delete

    public void Remove(int warehouseId, int orderId, string type)
    {
      Warehouses.Users.EnsureUser(warehouseId);
      var order = EnsureGet(warehouseId, orderId, type);
      if (order.status != OrderStatus.Created) {
        throw new FailureOperation("无法删除已完成的订单");
      }

      DbContext.Remove(order);
    }

    // Update

    public Order Update(Order order, string type)
    {
      EnsureRelationships(order);
      var oldOrder = EnsureGet(order.warehouse_id, order.id, type);

      if (order.status != OrderStatus.Created) {
        throw new FailureOperation("已完成的订单无法再次修改");
      }
      if (order.type != type) {
        throw new FailureOperation("订单类型错误");
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
      DbContext.Entry(oldOrder).Property(o => o.type).IsModified = false;
      DbContext.Entry(oldOrder).Property(o => o.status).IsModified = false;
      DbContext.Entry(oldOrder).Property(o => o.operator_id).IsModified = false;

      return oldOrder;
    }


    public void Finish(int warehouseId, int orderId, int locationId, string type)
    {
      Warehouses.Users.EnsureUser(warehouseId);

      var order = EnsureGet(warehouseId, orderId, type);

      if (order.status != OrderStatus.Created) {
        throw new FailureOperation("订单无法重新入库");
      }

      var location = Locations.EnsureGet(warehouseId, locationId);
      var quantities = new Dictionary<ValueTuple<int, int>, int>();
      var areaId = location.area_id;

      order.items.ForEach(item => {
        var key = (item.good_id, item.item_id);
        var quantity = item.projects.Aggregate(item.quantity, (s, p) => s - p.quantity);

        if (quantity == 0) return;

        quantities[key] = quantities.ContainsKey(key)
          ? quantities[key] + quantity : quantity;
      });

      foreach (var keyValue in quantities) {
        var quantity = keyValue.Value;
        var (goodId, itemId) = keyValue.Key;
        var stock = Stocks.GetOrNew(warehouseId, goodId, itemId, areaId, locationId);
        var records = new List<StockRecord>() {
          new StockRecord {
            quantity = quantity,
            order_id = order.id
          }
        };
        stock.quantity += quantity;
        stock.records = records;
        Stocks.Update(stock);
      }

      order.status = OrderStatus.Finished;
    }

    public void File(int warehouseId, int orderId, string type)
    {
      Warehouses.Users.EnsureUser(warehouseId);
      var order = EnsureGet(warehouseId, orderId, type);

      if (order.status != OrderStatus.Finished) {
        throw new FailureOperation("未完成的订单无法被归档");
      }

      order.status = OrderStatus.Filed;
    }

    public void Restore(int warehouseId, int orderId, string type)
    {
      Warehouses.Users.EnsureUser(warehouseId);
      var order = EnsureGet(warehouseId, orderId, type);

      if (order.status != OrderStatus.Filed) {
        throw new FailureOperation("未归档的订单无法被恢复");
      }

      order.status = OrderStatus.Finished;
    }

    // Select

    public Order EnsureGet(int warehouseId, int orderId, string type)
    {
      var order = Table
        .Include(o => o.items)
          .ThenInclude(oi => oi.invoice)
        .Include(o => o.items)
          .ThenInclude(oi => oi.projects)
        .Include(o => o.payments)
        .Where(o =>
          o.type == type &&
          o.id == orderId &&
          o.warehouse_id == warehouseId
        )
        .SingleOrDefault();

      if (order == null) {
        throw new FailureOperation("订单不存在");
      }

      return order;
    }

    public object Find(int warehouseId, int orderId, string type)
    {
      Warehouses.Users.EnsureUser(warehouseId);

      var order = EnsureGet(warehouseId, orderId, type);
      var goodIds = order.items.Select(item => item.good_id).ToArray();
      var itemIds = order.items.Select(item => item.item_id).ToArray();
      var projectIds = order.items.SelectMany(item => item.projects.Select(project => project.project_id)).ToArray();

      return new {
        order,
        applicant = DbContext.Find<User>(order.applicant_id),
        supplier = DbContext.Find<Supplier>(order.supplier_id),
        department = DbContext.Find<Department>(order.department_id),
        goods = Set<Good>().Where(good => goodIds.Contains(good.id)).ToRelationship(),
        items = Set<Item>().Where(item => itemIds.Contains(item.id)).ToRelationship(),
        projects = Set<Project>().Where(project => projectIds.Contains(project.id)).ToRelationship(),
      };
    }

    public IPagination<Order> Search(int warehouseId, string type, string status, string search, int page, int pageSize)
    {
      Warehouses.Users.EnsureUser(warehouseId);

      var itemIds = Enumerable.Empty<int>();
      var goodIds = Enumerable.Empty<int>();
      var projectIds = Enumerable.Empty<int>();
      var supplierIds = Enumerable.Empty<int>();
      var applicantIds = Enumerable.Empty<int>();
      var departmentIds = Enumerable.Empty<int>();

      if (search != null) {
        supplierIds = Set<Supplier>()
          .Where(s => s.warehouse_id == warehouseId && s.name.Contains(search))
          .Select(s => s.id);
        applicantIds = Set<WarehouseUser>()
          .Include(wu => wu.user)
          .Where(u => u.user.name.Contains(search))
          .Select(u => u.user.id);
        departmentIds = Set<Department>()
          .Where(d => d.warehouse_id == warehouseId && d.name.Contains(search))
          .Select(d => d.id);
      }

      var entities =  Set<Order>()
        .Include(order => order.items)
          .ThenInclude(order => order.invoice)
        .Include(order => order.items)
          .ThenInclude(order =>  order.projects)
        .Include(order => order.payments)
        .Where(order =>
          order.type == type &&
          order.warehouse_id == warehouseId &&
          (status == null ?
            order.status != OrderStatus.Filed:
            order.status == status) &&
          (search == null ? true : (
            supplierIds.Contains(order.supplier_id) ||
            applicantIds.Contains(order.applicant_id) ||
            departmentIds.Contains(order.department_id)
          ))
        )
        .OrderByDescending(order => order.created_at)
          .ThenBy(order => order.id)
        .Paginate(page, pageSize);

      supplierIds = entities.Data.Values.Select(order => order.supplier_id).Distinct();
      departmentIds = entities.Data.Values.Select(order => order.department_id).Distinct();
      itemIds = entities.Data.Values.SelectMany(order => order.items.Select(item => item.item_id)).Distinct();
      goodIds = entities.Data.Values.SelectMany(order => order.items.Select(item => item.good_id)).Distinct();
      applicantIds = entities.Data.Values.SelectMany(
        order => new int[] {
          order.operator_id,
          order.applicant_id
        }
      ).Distinct();
      projectIds = entities.Data.Values.SelectMany(
        order => order.items.SelectMany(
          item => item.projects.Select(
            project => project.project_id
          )
        )
      ).Distinct();

      entities.Relationships = new {
        users = Set<User>().Where(user => applicantIds.Contains(user.id)).ToRelationship(),
        goods = Set<Good>().IgnoreQueryFilters()
          .Where(good => goodIds.Contains(good.id)).ToRelationship(),
        items = Set<Item>().IgnoreQueryFilters()
          .Where(item => itemIds.Contains(item.id)).ToRelationship(),
        projects = Set<Project>().Where(project => projectIds.Contains(project.id)).ToRelationship(),
        suppliers = Set<Supplier>().Where(supplier => supplierIds.Contains(supplier.id)).ToRelationship(),
        departments = Set<Department>().Where(department => departmentIds.Contains(department.id)).ToRelationship(),
      };

      return entities;
    }

  }
}
