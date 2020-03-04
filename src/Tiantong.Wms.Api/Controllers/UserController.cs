using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class UserController : BaseController
  {
    private IAuth _auth;

    private IConfiguration _config;

    private UserRepository _users;

    public UserController(UserRepository users, IAuth auth, IConfiguration config)
    {
      _auth = auth;
      _users = users;
      _config = config;
    }

    // 初始化一个 root 用户
    // 用户 email 和 password 由配置文件提供
    // 若 root 用户已存在，则不再重复创建
    public object InitializeRootUser()
    {
      if (_users.HasRoot()) {
        return JsonMessage("Root user has been initialized");
      } else {
        var user = new User {
          type = UserTypes.Root,
          password = _config.GetValue("root_password", "123456"),
          email = _config.GetValue("root_email", "root@system.com"),
        };

        _users.Add(user);
        _users.UnitOfWork.SaveChanges();

        return JsonMessage("Success to initialize root user");
      }
    }

    public User[] Search()
    {
      _auth.EnsureType(UserTypes.Root);

      return _users.Search();
    }

    //

    public class RegisterUserParams
    {
      [Required]
      [EmailAddress]
      [UserEmailUnique]
      public string email { get; set; }

      [Required]
      [MaxLength(20)]
      public string password { get; set; }

      [MaxLength(20)]
      public string name { get; set; } = "";
    }

    public object Register([FromBody] RegisterUserParams param)
    {
      var user = new User();
      user.type = UserTypes.Owner;
      user.email = param.email;
      user.password = param.password;
      user.name = param.name;
      _users.Add(user);
      _users.UnitOfWork.SaveChanges();

      return JsonMessage("Success to register user");
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
      _auth.Ensure();

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
      var user = _users.FindByEmail(param.email);

      if (!_users.MatchUserPassword(user, param.password)) {
        throw new HttpException("email and password is not matched", 401);
      }

      var (token, expired_at, refresh_at) = _auth.Encode(user);

      return new { token, expired_at, refresh_at };
    }

    public object RefreshToken()
    {
      _auth.Ensure();

      var (token, expired_at, refresh_at) = _auth.Encode(_auth.User);

      return new { token, expired_at, refresh_at };
    }

  }
}
