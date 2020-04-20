using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class EmailVerificationController : BaseController
  {
    private EmailVerificationRepository _emails;

    public EmailVerificationController(EmailVerificationRepository emails)
    {
      _emails = emails;
    }

    public class VerifyParams
    {
      public int id { get; set; }

      public string code { get; set; }
    }

    public object Verify([FromBody] VerifyParams param)
    {
      _emails.Verify(param.id, param.code);
      _emails.UnitOfWork.SaveChanges();

      return SuccessOperation("邮箱验证码认证成功");
    }
  }
}
