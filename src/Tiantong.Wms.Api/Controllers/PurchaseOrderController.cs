using System.Linq;
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

    private ItemRepository _items;

    private ProjectRepository _projects;

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
      ProjectRepository projects,
      SupplierRepository suppliers,
      PurchaseOrderRepository orders,
      DepartmentRepository departments,
      WarehouseUserRepository warehouseUsers,
      WarehouseRepository warehouses
    ) {
      _auth = auth;
      _users = users;
      _goods = goods;
      _items = items;
      _orders = orders;
      _projects = projects;
      _suppliers = suppliers;
      _warehouses = warehouses;
      _departments = departments;
      _warehouseUsers = warehouseUsers;
    }

    public object Create([FromBody] PurchaseOrder param)
    {
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);

      _departments.Ensure(param.warehouse_id, param.department_id);
      if (_warehouseUsers.Get(param.warehouse_id, param.applicant_id) == null) {
        return FailureOperation("申请人不存在");
      }
      _suppliers.Ensure(param.warehouse_id, param.supplier_id);
      foreach (var item in param.items) {
        _items.Ensure(param.warehouse_id, item.good_id, item.item_id);
      }

      _orders.Add(param);
      _orders.UnitOfWork.SaveChanges();

      return SuccessOperation("录料单已创建", param.id);
    }

    public object Update([FromBody] PurchaseOrder order)
    {
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(order.warehouse_id, _auth.User.id);
      _orders.Ensure(order.warehouse_id, order.id);
      _departments.Ensure(order.warehouse_id, order.department_id);
      if (_warehouseUsers.Get(order.warehouse_id, order.applicant_id) == null) {
        return FailureOperation("申请人不存在");
      }
      _suppliers.Ensure(order.warehouse_id, order.supplier_id);
      foreach (var item in order.items) {
        _items.Ensure(order.warehouse_id, item.good_id, item.item_id);
      }

      _orders.Update(order);
      _orders.UnitOfWork.SaveChanges();

      return SuccessOperation("录料单已更新");
    }

    public class FindParams
    {
      public int warehouse_id { get; set; }

      public int id { get; set; }
    }

    public object Find([FromBody] FindParams param)
    {
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);
      var order = _orders.EnsureGet(param.warehouse_id, param.id);
      var goodIds = order.items.Select(item => item.good_id).ToArray();
      var itemIds = order.items.Select(item => item.item_id).ToArray();
      var projectIds = order.items.SelectMany(item => item.projects.Select(project => project.project_id)).ToArray();

      return new {
        order,
        applicant = _users.Get(order.applicant_id),
        supplier = _suppliers.Get(order.supplier_id),
        department = _departments.Get(order.department_id),
        goods = _goods.Table.Where(good => goodIds.Contains(good.id)).ToRelationship(),
        items = _items.Table.Where(item => itemIds.Contains(item.id)).ToRelationship(),
        projects = _projects.Table.Where(project => projectIds.Contains(project.id)).ToRelationship(),
      };
    }

    public class SearchParams : BaseSearchParams
    {
      [Nonzero]
      public int warehouse_id { get; set; }
    }

    public IPagination<PurchaseOrder> Search([FromBody] SearchParams param)
    {
      var entities =  _orders.Table
        .Include(order => order.items)
          .ThenInclude(order => order.finance)
        .Include(order => order.items)
          .ThenInclude(order =>  order.projects)
        .Include(order => order.payments)
        .Where(order => order.warehouse_id == param.warehouse_id)
        .OrderByDescending(order => order.created_at)
          .ThenBy(order => order.id)
        .Paginate(param.page, param.page_size);

      var supplierIds = entities.Data.Values.Select(order => order.supplier_id).Distinct();
      var departmentIds = entities.Data.Values.Select(order => order.department_id).Distinct();
      var itemIds = entities.Data.Values.SelectMany(order => order.items.Select(item => item.item_id)).Distinct();
      var goodIds = entities.Data.Values.SelectMany(order => order.items.Select(item => item.good_id)).Distinct();
      var userIds = entities.Data.Values.SelectMany(
        order => new int[] {
          order.operator_id,
          order.applicant_id
        }
      ).Distinct();
      var projectIds = entities.Data.Values.SelectMany(
        order => order.items.SelectMany(
          item => item.projects.Select(
            project => project.project_id
          )
        )
      ).Distinct();

      entities.Relationships = new {
        users = _users.Table.Where(user => userIds.Contains(user.id)).ToRelationship(),
        goods = _goods.Table.IgnoreQueryFilters()
          .Where(good => goodIds.Contains(good.id)).ToRelationship(),
        items = _items.Table.IgnoreQueryFilters()
          .Where(item => itemIds.Contains(item.id)).ToRelationship(),
        projects = _projects.Table.Where(project => projectIds.Contains(project.id)).ToRelationship(),
        suppliers = _suppliers.Table.Where(supplier => supplierIds.Contains(supplier.id)).ToRelationship(),
        departments = _departments.Table.Where(department => departmentIds.Contains(department.id)).ToRelationship(),
      };

      return entities;
    }

  }
}
