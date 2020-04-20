using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class PasswordResetController : BaseController
  {
    private Mail _mail;

    private PasswordResetRepository _passwordResets;

    public PasswordResetController(PasswordResetRepository passwordResets, Mail mail)
    {
      _mail = mail;
      _passwordResets = passwordResets;
    }

    public class CreateByEmailParams
    {
      [EmailAddress(ErrorMessage = "邮箱地址无效")]
      public string email { get; set; }
    }

    public object CreateByEmail([FromBody] CreateByEmailParams param)
    {
      var entity = _passwordResets.CreateByEmail(param.email, 1800);
      _passwordResets.UnitOfWork.SaveChanges();
      _mail.Send(param.email, entity.email_verification.code);

      return SuccessOperation("验证码已发送至您的邮箱", entity.id);
    }

    public class SubmitByEmailParams
    {
      public int id { get; set; }

      public string code { get; set; }

      public string password { get; set; }
    }

    public object SubmitByEmail([FromBody] SubmitByEmailParams param)
    {
      _passwordResets.SubmitByEmail(param.id, param.code, param.password);
      _passwordResets.UnitOfWork.SaveChanges();

      return SuccessOperation("密码修改成功");
    }
  }
}
