using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class WarehouseUserController : BaseController
  {
    private WarehouseUserRepository _warehouseUsers;

    public WarehouseUserController(WarehouseRepository warehouses)
    {
      _warehouseUsers = warehouses.Users;
    }

    public object Create([FromBody] WarehouseUser wu)
    {
      _warehouseUsers.Add(wu);
      _warehouseUsers.UnitOfWork.SaveChanges();

      return SuccessOperation("用户已添加", wu.id);
    }

    public class DeleteParams
    {
      public int id { get; set; }
    }

    public object Delete([FromBody] DeleteParams param)
    {
      _warehouseUsers.Remove(param.id);
      _warehouseUsers.UnitOfWork.SaveChanges();

      return SuccessOperation("用户已删除");
    }

    public object Update([FromBody] WarehouseUser wu)
    {
      _warehouseUsers.Update(wu);
      _warehouseUsers.UnitOfWork.SaveChanges();

      return SuccessOperation("用户已更新");
    }

    public class FindParams
    {
      public int id { get; set; }
    }

    public WarehouseUser Find([FromBody] FindParams param)
    {
      return _warehouseUsers.Find(param.id);
    }

    public class AllParams
    {
      public int warehouse_id { get; set; }

      public string search { get; set; } = "";
    }

    public IEntities<WarehouseUser, int> All([FromBody] AllParams param)
    {
      return _warehouseUsers.SearchAll(param.warehouse_id, param.search);
    }
  }
}
