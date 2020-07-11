using Microsoft.AspNetCore.Mvc;
using Renet;
using Renet.Utils;
using Renet.Web;
using Renet.Web.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Tiantong.Account.Utils;

namespace Tiantong.Account.Api
{
  public class UserController: BaseController
  {
    private AccountContext _account;

    private EmailVerificationService _emailVerification;

    private IHash _hash;

    private JWT _jwt;

    public UserController(
      AccountContext account,
      EmailVerificationService emailVerification,
      IHash hash,
      JWT jwt
    ) {
      _account = account;
      _emailVerification = emailVerification;
      _hash = hash;
      _jwt = jwt;
    }

    public class RegisterByEmailParams
    {
      [StringRange(1, 30, ErrorMessage = "名字长度必须在 1~30 之间")]
      public string name { get; set; }

      [Required(ErrorMessage = "邮箱地址不能为空")]
      [EmailAddress(ErrorMessage = "邮箱地址格式错误")]
      public string email { get; set; }

      [StringRange(8, 20, ErrorMessage = "密码长度必须在 6~20 之间")]
      public string password { get; set; }

      public string verification_key { get; set; }

      public string verification_code { get; set; }
    }

    [HttpPost]
    [Route("/register/email")]
    public async Task<ActionResult<object>> RegisterByEmail([FromBody] RegisterByEmailParams param)
    {
      if (_account.Users.Any(u => u.email == param.email)) {
        throw KnownException.Error("邮箱已被注册", 409);
      }

      await _emailVerification.VerifyAsync(param.email, param.verification_key, param.verification_code);

      var user = new User {
        name = param.name,
        email = param.email,
        password = _hash.Make(param.password),
      };

      _account.Add(user);
      _account.SaveChanges();

      // 注册后自动登陆，不使用 text
      return new {
        text = "注册完成"
      };
    }

    public class ResetPasswordParams
    {
      [Required(ErrorMessage = "邮箱地址不能为空")]
      [EmailAddress(ErrorMessage = "邮箱地址格式错误")]
      public string email { get; set; }

      [StringRange(8, 20, ErrorMessage = "密码长度必须在 6~20 之间")]
      public string password { get; set; }

      public string verification_key { get; set; }

      public string verification_code { get; set; }
    }

    [HttpPost]
    [Route("/password-reset/email")]
    public async Task<ActionResult<object>> ResetPasswordByEmail([FromBody] ResetPasswordParams param)
    {
      await _emailVerification.VerifyAsync(param.email, param.verification_key, param.verification_code);

      var user = _account.Users.FirstOrDefault(u => u.email == param.email);

      if (user == null) {
        throw KnownException.Error("用户不存在", 404);
      }

      user.password = _hash.Make(param.password);
      _account.SaveChanges();

      return new {
        message = "密码已修改"
      };
    }

    public class ConfirmPasswordParams
    {
      public string message { get; set; }

      public string password { get; set; }
    }

    [HttpPost]
    [Route("/password/confirm")]
    public object ConfirmPassword(
      [FromHeader] string authorization,
      [FromBody] ConfirmPasswordParams param
    ) {
      var (id, _, _) = _jwt.Parse(authorization);

      var user = _account.Users.FirstOrDefault(u => u.id == id);

      if (user is null) {
        throw KnownException.Error("用户不存在");
      }

      if (!_hash.Match(param.password, user.password)) {
        throw KnownException.Error("密码确认失败");
      }

      return new {
        message = param.message ?? "密码已确认"
      };
    }

    public class LoginByEmailParams
    {
      [Required(ErrorMessage = "邮箱地址不能为空")]
      [EmailAddress(ErrorMessage = "邮箱地址格式错误")]
      public string email { get; set; }

      public string password { get; set; }
    }

    [HttpPost]
    [Route("login/email")]
    public ActionResult<object> LoginByEmail([FromBody] LoginByEmailParams param)
    {
      var user = _account.Users.FirstOrDefault(u => u.email == param.email);

      if (user is null || !_hash.Match(param.password, user.password)) {
        throw KnownException.Error("邮箱或密码错误", 401);
      }

      var (token, exp, rfa) = _jwt.Encode(user.id);

      return new {
        message = "登陆成功",
        token = token,
        expired_at = exp,
        refresh_at = rfa,
      };
    }

    [HttpPost]
    [Route("/token/verify")]
    public ActionResult<object> VerifyToken([FromHeader] string authorization)
    {
      var (id, _, _) = _jwt.Parse(authorization);

      if (!_account.Users.Any(u => u.id == id)) {
        throw KnownException.Error("token 信息不存在", 404);
      }

      return new {
        message = "token 验证通过",
        id = id,
      };
    }

    [HttpPost]
    [Route("/token/refresh")]
    public ActionResult<object> RefreshToken([FromHeader] string authorization)
    {
      var (id, rfa, _) = _jwt.Decode(authorization);

      if (DateTime.Now > rfa) {
        throw KnownException.Error("token 已过期并无法刷新", 403);
      }

      var (token, refresh_at, expired_at) = _jwt.Encode(id);

      return new {
        token,
        expired_at,
        refresh_at,
      };
    }

    [HttpPost]
    [Route("/person/data")]
    public ActionResult<object> GetUserInfo([FromHeader] string authorization)
    {
      var (id, _, _) = _jwt.Parse(authorization);
      var user = _account.Users.FirstOrDefault(u => u.id == id);

      if (user == null) {
        throw KnownException.Error("用户信息不存在", 404);
      }

      return new {
        id = user.id,
        name = user.name,
        email = user.email,
        avatar_url = user.avatar_url,
      };
    }

    public class UpdatePersonParams
    {
      [StringRange(1, 30, ErrorMessage = "名字长度必须在 1~30 之间")]
      public string name { get; set; }
    }

    [HttpPost]
    [Route("/person/update")]
    public ActionResult<object> UpdatePerson([FromHeader] string authorization, [FromBody] UpdatePersonParams param)
    {
      var (id, _, _) = _jwt.Parse(authorization);
      var user = _account.Users.FirstOrDefault(u => u.id == id);

      if (user == null) {
        throw KnownException.Error("用户不存在", 404);
      }

      user.name = param.name;
      _account.SaveChanges();

      return new {
        message = "用户信息已更新"
      };
    }

  }
}
