using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class DepartmentController : BaseController
  {
    private Auth _auth;

    private DepartmentRepository _departments;

    private WarehouseRepository _warehouses;

    public DepartmentController(
      Auth auth,
      WarehouseRepository warehouses,
      DepartmentRepository departments
    ) {
      _auth = auth;
      _warehouses = warehouses;
      _departments = departments;
    }

    public object Create([FromBody] Department department)
    {
      _auth.EnsureUser();
      _warehouses.EnsureUser(department.warehouse_id, _auth.User.id);
      _departments.EnsureUnique(department);

      _departments.Add(department);
      _departments.UnitOfWork.SaveChanges();

      return SuccessOperation("部门已创建", department.id);
    }

    public object Update([FromBody] Department department)
    {
      _auth.EnsureUser();
      _warehouses.EnsureUser(department.warehouse_id, _auth.User.id);

      _departments.EnsureUnique(department);
      _departments.Update(department);
      _departments.UnitOfWork.SaveChanges();

      return SuccessOperation("供应商信息已保存");
    }

    public class RemoveParams
    {
      public int id { get; set; }
    }

    public object Remove([FromBody] RemoveParams param)
    {
      _auth.EnsureUser();
      var department = _departments.EnsureGet(param.id);
      _warehouses.EnsureUser(department.warehouse_id, _auth.User.id);
      _departments.Remove(department);
      _departments.UnitOfWork.SaveChanges();

      return SuccessOperation("部门已删除");
    }

    public class FindParams
    {
      public int id { get; set; }
    }

    public Department Find([FromBody] FindParams param)
    {
      _auth.EnsureUser();
      var department = _departments.EnsureGet(param.id);
      _warehouses.EnsureUser(department.warehouse_id, _auth.User.id);

      return department;
    }

    public class AllParams
    {
      public int warehouse_id { get; set; }

      public string search { get; set; } = "";
    }

    public IEntities<Department, int> All([FromBody] AllParams param)
    {
      _auth.EnsureUser();
      _warehouses.EnsureUser(param.warehouse_id, _auth.User.id);

      return _departments.Table
        .Where(department =>
          (department.warehouse_id == param.warehouse_id) &&
          (
            param.search == null ? true :
            department.name.Contains(param.search)
          )
        )
        .ToEntities();
    }
  }
}
