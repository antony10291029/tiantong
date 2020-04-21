using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class WarehouseController : BaseController
  {
    private WarehouseRepository _warehouses;

    public WarehouseController(WarehouseRepository warehouses) {

      _warehouses = warehouses;
    }

    public object Create([FromBody] Warehouse warehouse)
    {
      _warehouses.Add(warehouse);

      return SuccessOperation("仓库已创建", warehouse.id);
    }

    public class DeleteParams
    {
      public int warehouse_id { get; set; }
    }

    public object Delete([FromBody] DeleteParams param)
    {
      return FailureOperation("目前不允许删除仓库");
    }

    public object Update([FromBody] Warehouse warehouse)
    {
      _warehouses.Update(warehouse);
      _warehouses.UnitOfWork.SaveChanges();

      return SuccessOperation("仓库设置已保存");
    }

    public Warehouse[] Search()
    {
      return _warehouses.Search();
    }

    public class FindParams
    {
      public int id {get; set; }
    }

    public Warehouse Find([FromBody] FindParams param)
    {
      return _warehouses.Find(param.id);
    }
  }
}
