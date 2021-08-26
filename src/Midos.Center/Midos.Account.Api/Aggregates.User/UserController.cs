using Microsoft.AspNetCore.Mvc;
using Midos.App;
using Midos.Domain;

namespace Midos.Account.Api
{
  public class UserController
  {
    private readonly UserDomainService _service;

    public UserController(UserDomainService service)
    {
      _service = service;
    }

    [HttpPost("/users/add")]
    public object Add([FromBody] User user)
    {
      _service.Add(user);

      return new { message = "用户已添加" };
    }

    public record RemoveParams<User>
    {
      public long Id { get; set; }
    }

    [HttpPost("/users/delete")]
    public object Delete([FromBody] RemoveParams<User> param)
    {
      _service.Delete(param.Id);

      return new { message = "用户已删除" };
    }

    [HttpPost("/users/update")]
    public object Update([FromBody] User user)
    {
      _service.Update(user);

      return new { message = "用户已删除" };
    }

    public record FindUserParams
    {
      public long Id { get; set; }
    }

    [HttpPost("/users/find")]
    public object Find([FromBody] FindUserParams param)
    {
      return _service.Find(param.Id);
    }

    [HttpPost("/users/query")]
    public object Query([FromBody] QueryParams param)
    {
      return _service.Query(param);
    }

    [HttpPost("/users/paginate")]
    public object Paginate([FromBody] PaginateParams param)
    {
      return _service.Paginate(param);
    }
  }
}
