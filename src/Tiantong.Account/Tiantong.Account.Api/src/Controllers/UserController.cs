using Microsoft.AspNetCore.Mvc;
using Renet;
using Renet.Utils;
using Renet.Web;
using Renet.Web.Attributes;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Tiantong.Account.Api
{
  [Route("/users")]
  public class UserController: BaseController
  {
    private AccountContext _account;

    private IHash _hash;

    public UserController(
      AccountContext account,
      IHash hash
    ) {
      _account = account;
      _hash = hash;
    }

    public class RegisterByEmailParams
    {
      [StringRange(1, 30, ErrorMessage = "名字长度必须在 1~30 之间")]
      public string name { get; set; }

      [EmailAddress(ErrorMessage = "邮箱地址格式错误")]
      public string email { get; set; }

      [StringRange(8, 30, ErrorMessage = "密码长度必须在 6~80 之间")]
      public string password { get; set; }

      [StringRange(10, 10, ErrorMessage = "邮箱验证格式错误")]
      public string verification_key { get; set; }

      [StringRange(6, 6, ErrorMessage = "邮箱验证码格式错误")]
      public string verification_code { get; set; }
    }

    public ActionResult<object> RegisterByEmail([FromBody] RegisterByEmailParams param)
    {
      if (_account.Users.Any(u => u.email == param.email)) {
        throw KnownException.Error("邮箱已被注册", 409);
      }

      var user = new User {
        name = param.name,
        email = param.email,
        password = _hash.Make(param.password),
      };

      _account.Add(user);
      _account.SaveChanges();

      return new {
        message = "注册完成"
      };
    }

  }
}
