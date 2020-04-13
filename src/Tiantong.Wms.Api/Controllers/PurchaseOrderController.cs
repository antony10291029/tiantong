using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class PurchaseOrderController : BaseController
  {
    private IAuth _auth;

    private UserRepository _users;

    private GoodRepository _goods;

    private StockRepository _stocks;

    private ProjectRepository _projects;

    private LocationRepository _locations;

    private SupplierRepository _suppliers;

    private PurchaseOrderRepository _orders;

    private WarehouseRepository _warehouses;

    private DepartmentRepository _departments;

    private WarehouseUserRepository _warehouseUsers;

    public PurchaseOrderController(
      IAuth auth,
      UserRepository users,
      ItemRepository items,
      GoodRepository goods,
      StockRepository stocks,
      ProjectRepository projects,
      LocationRepository locations,
      SupplierRepository suppliers,
      PurchaseOrderRepository orders,
      DepartmentRepository departments,
      WarehouseUserRepository warehouseUsers,
      WarehouseRepository warehouses
    ) {
      _auth = auth;
      _users = users;
      _goods = goods;
      _orders = orders;
      _stocks = stocks;
      _projects = projects;
      _locations = locations;
      _suppliers = suppliers;
      _warehouses = warehouses;
      _departments = departments;
      _warehouseUsers = warehouseUsers;
    }

    private void EnsureOrder(PurchaseOrder order)
    {
      _departments.EnsureExists(order.warehouse_id, order.department_id);
      if (_warehouseUsers.Get(order.warehouse_id, order.applicant_id) == null) {
        throw new FailureOperation("申请人不存在");
      }
      _suppliers.Ensure(order.warehouse_id, order.supplier_id);
      foreach (var item in order.items) {
        _goods.Items.EnsureExists(order.warehouse_id, item.good_id, item.item_id);
      }
    }

    public object Create([FromBody] PurchaseOrder order)
    {
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(order.warehouse_id, _auth.User.id);

      EnsureOrder(order);
      _orders.Add(order);
      _orders.UnitOfWork.SaveChanges();

      return SuccessOperation("录料单已创建", order.id);
    }

    public class FindParams
    {
      public int order_id { get; set; }

      public int warehouse_id { get; set; }
    }

    public object Delete([FromBody] FindParams param)
    {
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);
      var order = _orders.EnsureGet(param.warehouse_id, param.order_id);

      if (order.status != PurchaseOrderStatuses.Created) {
        return FailureOperation("无法删除已完成的订单");
      }
      _orders.Remove(order);
      _orders.UnitOfWork.SaveChanges();

      return SuccessOperation("录料单已删除");
    }

    public object Update([FromBody] PurchaseOrder order)
    {
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(order.warehouse_id, _auth.User.id);

      EnsureOrder(order);
      _orders.Update(order);
      _orders.UnitOfWork.SaveChanges();

      return SuccessOperation("录料单已更新");
    }

    public object Finish([FromBody] FinishParams param)
    {
      var warehouseId = param.warehouse_id;
      _warehouses.EnsureOwner(warehouseId, _auth.User.id);
      var order = _orders.EnsureGet(warehouseId, param.order_id);
      if (order.status == PurchaseOrderStatuses.Finished) {
        return FailureOperation("录料单无法重复入库");
      }
      var location = _locations.EnsureGet(warehouseId, param.location_id);
      var quantities = new Dictionary<ValueTuple<int, int>, int>();
      var areaId = location.area_id;
      var locationId = location.id;

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
        var stock = _stocks.GetOrNew(warehouseId, goodId, itemId, areaId, locationId);
        var records = new List<StockRecord>() {
          new StockRecord {
            quantity = quantity,
            order_type = OrderTypes.Purchase,
            order_number = order.id.ToString(),
          }
        };
        stock.quantity += quantity;
        stock.records = records;
        _stocks.Update(stock);
      }

      order.status = PurchaseOrderStatuses.Finished;
      _orders.UnitOfWork.SaveChanges();

      return SuccessOperation("录料单已确认入库");
    }

    public object File([FromBody] FindParams param)
    {
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);
      var order = _orders.EnsureGet(param.warehouse_id,  param.order_id);

      if (order.status != PurchaseOrderStatuses.Finished) {
        return FailureOperation("未入库的录料单无法被归档");
      }
      order.status = PurchaseOrderStatuses.Filed;
      _orders.UnitOfWork.SaveChanges();

      return SuccessOperation("录料单已归档");
    }

    public object Restore([FromBody] FindParams param)
    {
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);
      var order = _orders.EnsureGet(param.warehouse_id,  param.order_id);

      if (order.status != PurchaseOrderStatuses.Filed) {
        return FailureOperation("未归档的录料单无法恢复");
      }
      order.status = PurchaseOrderStatuses.Finished;
      _orders.UnitOfWork.SaveChanges();

      return SuccessOperation("录料单已恢复");
    }

    public class FinishParams
    {
      public int warehouse_id { get; set; }

      public int location_id { get; set; }

      public int order_id { get; set; }
    }

    public object Find([FromBody] FindParams param)
    {
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);
      var order = _orders.EnsureGet(param.warehouse_id, param.order_id);
      var goodIds = order.items.Select(item => item.good_id).ToArray();
      var itemIds = order.items.Select(item => item.item_id).ToArray();
      var projectIds = order.items.SelectMany(item => item.projects.Select(project => project.project_id)).ToArray();

      return new {
        order,
        applicant = _users.Get(order.applicant_id),
        supplier = _suppliers.Get(order.supplier_id),
        department = _departments.Get(order.department_id),
        goods = _goods.Table.Where(good => goodIds.Contains(good.id)).ToRelationship(),
        items = _goods.Items.Table.Where(item => itemIds.Contains(item.id)).ToRelationship(),
        projects = _projects.Table.Where(project => projectIds.Contains(project.id)).ToRelationship(),
      };
    }

    public class SearchParams : BaseSearchParams
    {
      public int warehouse_id { get; set; }

      public string status { get; set; }
    }

    public IPagination<PurchaseOrder> Search([FromBody] SearchParams param)
    {
      var itemIds = Enumerable.Empty<int>();
      var goodIds = Enumerable.Empty<int>();
      var projectIds = Enumerable.Empty<int>();
      var supplierIds = Enumerable.Empty<int>();
      var applicantIds = Enumerable.Empty<int>();
      var departmentIds = Enumerable.Empty<int>();

      if (param.search != null) {
        supplierIds = _suppliers.Table
          .Where(s => s.warehouse_id == param.warehouse_id && s.name.Contains(param.search))
          .Select(s => s.id);
        applicantIds = _warehouseUsers.Table.Include(wu => wu.user)
          .Where(u => u.user.name.Contains(param.search))
          .Select(u => u.user.id);
        departmentIds = _departments.Table
          .Where(d => d.warehouse_id == param.warehouse_id && d.name.Contains(param.search))
          .Select(d => d.id);
      }

      var entities =  _orders.Table
        .Include(order => order.items)
          .ThenInclude(order => order.finance)
        .Include(order => order.items)
          .ThenInclude(order =>  order.projects)
        .Include(order => order.payments)
        .Where(order =>
          order.warehouse_id == param.warehouse_id &&
          (param.status == null ?
            order.status != PurchaseOrderStatuses.Filed:
            order.status == param.status) &&
          (param.search == null ? true : (
            supplierIds.Contains(order.supplier_id) ||
            applicantIds.Contains(order.applicant_id) ||
            departmentIds.Contains(order.department_id)
          ))
        )
        .OrderByDescending(order => order.created_at)
          .ThenBy(order => order.id)
        .Paginate(param.page, param.page_size);

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
        users = _users.Table.Where(user => applicantIds.Contains(user.id)).ToRelationship(),
        goods = _goods.Table.IgnoreQueryFilters()
          .Where(good => goodIds.Contains(good.id)).ToRelationship(),
        items = _goods.Items.Table.IgnoreQueryFilters()
          .Where(item => itemIds.Contains(item.id)).ToRelationship(),
        projects = _projects.Table.Where(project => projectIds.Contains(project.id)).ToRelationship(),
        suppliers = _suppliers.Table.Where(supplier => supplierIds.Contains(supplier.id)).ToRelationship(),
        departments = _departments.Table.Where(department => departmentIds.Contains(department.id)).ToRelationship(),
      };

      return entities;
    }

  }
}
