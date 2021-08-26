using Microsoft.AspNetCore.Mvc;
using Midos.App;
using Midos.Domain;

namespace Midos.Account.Api
{
  public class RoleController
  {
    private readonly RoleDomainService _service;

    public RoleController(RoleDomainService service)
    {
      _service = service;
    }

    [HttpPost("/roles/add")]
    public object Add([FromBody] Role role)
    {
      _service.Add(role);

      return new { message = "角色已添加" };
    }

    [HttpPost("/roles/update")]
    public object Update([FromBody] Role role)
    {
      _service.Update(role);

      return new { message = "角色已更新" };
    }

    public class RemoveParams<Role>
    {
      public long Id { get; set; }
    }

    [HttpPost("/roles/remove")]
    public object Remove([FromBody] RemoveParams<Role> param)
    {
      _service.Remove(param.Id);

      return new { message = "角色已删除" };
    }

    [HttpPost("/roles/query")]
    public object Query([FromBody] QueryParams param) =>
      _service.Query(param);
  }
}
