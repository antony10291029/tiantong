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
  [Route("/email-verification")]
  public class EmailVerificationController: BaseController
  {
    private AccountContext _account;

    private IRandom _random;

    private Mail _mail;

    public EmailVerificationController(
      Mail mail,
      IRandom random,
      AccountContext account
    ) {
      _mail = mail;
      _random = random;
      _account = account;
    }

    public class SendParams
    {
      [Required(ErrorMessage = "邮箱地址不能为空")]
      [EmailAddress(ErrorMessage = "邮箱地址格式错误")]
      public string address { get; set; }

      [StringRange(1, 50, ErrorMessage = "邮件主题长度必须在 1～50 之间")]
      public string subject { get; set; }

      [Range(0, 172800, ErrorMessage = "验证码有效期最多为 2 天")]
      public int duration { get; set; }
    }

    [HttpPost]
    [Route("send")]
    public async Task<ActionResult<object>> Send([FromBody] SendParams param)
    {
      var key = _random.String(10);
      var code = _random.Int(100000, 999999).ToString();
      var content = $"尊敬的用户，您正在使用 <strong>{param.subject}</strong> 服务, 本次验证码为: <strong>{code}</strong>";

      try {
        await _mail.SendAsync(param.address, "验证邮件", content);
      } catch {
        throw KnownException.Error("邮件发送失败");
      }

      var emailVerification = new EmailVerification {
        address = param.address,
        error_count = 0,
        key = key,
        code = code,
        expired_at = DateTime.Now
      };
      _account.Add(emailVerification);
      _account.SaveChanges();

      return new {
        message = "邮件已发送",
        key = key,
      };
    }

    public class VerifyParams
    {
      [Required(ErrorMessage = "邮箱地址不能为空")]
      [EmailAddress(ErrorMessage = "邮箱地址格式错误")]
      public string address { get; set; }

      [Required(ErrorMessage = "Key 不能为空")]
      public string key { get; set; }

      [Required(ErrorMessage = "Code 不能为空")]
      public string code { get; set; }
    }

    [HttpPost]
    [Route("verify")]
    public ActionResult<object> Verify([FromBody] VerifyParams param)
    {
      var ev = _account.EmailVerifications.FirstOrDefault(ev =>
        ev.address == param.address &&
        ev.key == param.key
      );

      if (ev == null) {
        throw KnownException.Error("验证信息不存在", 404);
      } if (ev.expired_at > DateTime.Now) {
        _account.Remove(ev);
        _account.SaveChanges();

        throw KnownException.Error("认证已超时", 403);
      } else if (ev.error_count == 5) {
        _account.Remove(ev);
        _account.SaveChanges();

        throw KnownException.Error("验证次数达到上限，请重新发送确认邮件", 403);
      } else if (ev.code != param.code) {
        ev.error_count += 1;
        _account.SaveChanges();

        throw KnownException.Error("验证码错误", 401);
      } else {
        _account.Remove(ev);
        _account.SaveChanges();

        return new {
          message = "邮箱验证通过",
        };
      }
    }

  }
}
