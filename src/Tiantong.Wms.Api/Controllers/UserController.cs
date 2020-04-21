using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class UserController : BaseController
  {
    private Auth _auth;

    private UserRepository _users;

    public UserController(UserRepository users, Auth auth)
    {
      _auth = auth;
      _users = users;
    }

    public class SearchParams
    {
      public int page { get; set; }

      public int page_size { get; set; }
    }

    public IPagination<User> Search([FromBody] SearchParams param)
    {
      _auth.EnsureType(UserType.Root);

      return _users.Table.Paginate(param.page, param.page_size);
    }

    //

    public class RegisterParams: User
    {
      [Required]
      [MaxLength(20)]
      public override string password { get; set; }
    }

    public object Register([FromBody] RegisterParams user)
    {
      _users.Register(user);
      _users.UnitOfWork.SaveChanges();

      return SuccessOperation("注册成功");
    }

    //

    public class UpdatePersonParams
    {
      [EmailAddress]
      [UserEmailUnique]
      public string email { get; set; }

      [MaxLength(20)]
      public string name { get; set; }

      [MaxLength(20)]
      public string password { get; set; }
    }

    public object UpdatePerson([FromBody] UpdatePersonParams param)
    {
      var user = _auth.User;

      if (param.email != null)  user.email = param.email;
      if (param.name != null) user.name = param.name;
      if (param.password != null) {
        user.password = param.password;
        _users.EncodePassword(user);
      }

      _users.UnitOfWork.SaveChanges();

      return JsonMessage("Success to update user");
    }

    public User GetPersonProfile()
    {
      _auth.EnsureValid();

      return _auth.User;
    }

    //

    public class AuthEmailParams {
      [Required]
      [EmailAddress]
      public string email { get; set; }

      [Required]
      public string password { get; set; }
    }

    public object AuthByEmail([FromBody] AuthEmailParams param)
    {
      var user = _users.EnsureGetByEmail(param.email);

      if (!_users.MatchUserPassword(user, param.password)) {
        throw new HttpException("登陆失败，账号或密码错误", 400);
      }

      var (token, expired_at, refresh_at) = _auth.Encode(user);

      return SuccessOperation(new {
        token,
        expired_at,
        refresh_at,
        message = "登陆成功"
      });
    }

    public object RefreshToken()
    {
      _auth.EnsureValid();

      var (token, expired_at, refresh_at) = _auth.Encode(_auth.User);

      return new { token, expired_at, refresh_at };
    }

  }
}
