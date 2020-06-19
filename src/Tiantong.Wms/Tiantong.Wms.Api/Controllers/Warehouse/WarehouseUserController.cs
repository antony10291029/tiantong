using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  [Route("/warehouses/users")]
  public class WarehouseUserController : BaseController
  {
    private WarehouseUserRepository _warehouseUsers;

    public WarehouseUserController(WarehouseRepository warehouses)
    {
      _warehouseUsers = warehouses.Users;
    }

    public class InviteParams
    {
      public int warehouse_id { get; set; }

      public int department_id { get; set; }

      public string email { get; set; }
    }

    [HttpPost]
    [Route("invite")]
    public object Invite([FromBody] InviteParams param)
    {
      var wu = _warehouseUsers.Invite(param.warehouse_id, param.department_id, param.email);
      _warehouseUsers.UnitOfWork.SaveChanges();

      return SuccessOperation("邀请成功，等待对方确认中");
    }

    public class CreateParams
    {
      public int warehouse_id { get; set; }

      public int department_id { get; set; }

      public string email { get; set; }

      public string name { get; set; } 
    }

    [HttpPost]
    [Route("create")]
    public object Create([FromBody] CreateParams param)
    {
      var wu = _warehouseUsers.Add(param.warehouse_id, param.department_id, param.email, param.name);
      _warehouseUsers.UnitOfWork.SaveChanges();

      return SuccessOperation("用户已添加", wu.id);
    }

    public class DeleteParams
    {
      public int id { get; set; }
    }

    [HttpPost]
    [Route("delete")]
    public object Delete([FromBody] DeleteParams param)
    {
      _warehouseUsers.Remove(param.id);
      _warehouseUsers.UnitOfWork.SaveChanges();

      return SuccessOperation("用户已删除");
    }

    [HttpPost]
    [Route("update")]
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

    [HttpPost]
    [Route("find")]
    public WarehouseUser Find([FromBody] FindParams param)
    {
      return _warehouseUsers.Find(param.id);
    }

    public class PersonParams
    {
      public int warehouse_id { get; set; }
    }

    public class EmailParams
    {
      [EmailAddress(ErrorMessage = "邮箱地址不合法")]
      public string email {  get; set; }
    }

    [HttpPost]
    [Route("person")]
    public WarehouseUser Person([FromBody] PersonParams param)
    {
      return _warehouseUsers.Person(param.warehouse_id);
    }

    public class AllParams
    {
      public int warehouse_id { get; set; }

      public string search { get; set; } = "";
    }

    [HttpPost]
    [Route("all")]
    public IEntities<WarehouseUser, int> All([FromBody] AllParams param)
    {
      return _warehouseUsers.SearchAll(param.warehouse_id, param.search);
    }
  }
}
