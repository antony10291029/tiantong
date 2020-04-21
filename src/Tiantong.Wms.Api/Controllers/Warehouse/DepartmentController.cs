using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class DepartmentController : BaseController
  {
    private DepartmentRepository _departments;

    public DepartmentController(WarehouseRepository warehouses)
    {
      _departments = warehouses.Departments;
    }

    public object Create([FromBody] Department department)
    {
      _departments.Add(department);
      _departments.UnitOfWork.SaveChanges();

      return SuccessOperation("部门已创建", department.id);
    }

    public object Update([FromBody] Department department)
    {
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
      _departments.Remove(param.id);
      _departments.UnitOfWork.SaveChanges();

      return SuccessOperation("部门已删除");
    }

    public class FindParams
    {
      public int id { get; set; }
    }

    public Department Find([FromBody] FindParams param)
    {
      return _departments.Find(param.id);
    }

    public class AllParams
    {
      public int warehouse_id { get; set; }

      public string search { get; set; } = "";

      public string type { get; set; }
    }

    public IEntities<Department, int> All([FromBody] AllParams param)
    {
      return _departments.All(param.warehouse_id, param.search, param.type);
    }
  }
}
