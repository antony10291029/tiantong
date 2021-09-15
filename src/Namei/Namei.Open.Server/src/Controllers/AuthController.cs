using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Security;

namespace Namei.Open.Server
{
  public class AuthController: ControllerBase
  {
    private readonly Dictionary<string, string> _accounts = new() {
      { "admin@namei", "namei@123" },
      { "namei_kxf", "password@8315" },
      { "namei_lh", "password@4167" },
      { "namei_lcl", "password@2633" },
      { "namei_lzp", "password@5190" }
    };

    public record LoginParams
    {
      public string Username { get; set; }

      public string Password { get; set; }
    }

    [HttpPost("/qrcode/login")]
    public object Login([FromBody] LoginParams param)
    {
      var isVerified = _accounts.ContainsKey(param.Username) && _accounts[param.Username] == param.Password;

      if (isVerified) {
        return Ok(new {
          message = "登录成功"
        });
      } else {
        return BadRequest(new {
          message = "登录失败，请检查用户名和密码"
        });
      }
    }
  }
}
