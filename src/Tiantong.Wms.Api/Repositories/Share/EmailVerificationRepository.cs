using System;
using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class EmailVerificationRepository : Repository<EmailVerification, int>
  {
    private IRandom _random;

    public EmailVerificationRepository(DbContext db, IRandom random): base(db)
    {
      _random = random;
    }

    public EmailVerification New(string email, int seconds = 1800)
    {
      var entity = new EmailVerification {
        email = email,
        code = _random.Int(100000, 999999).ToString(),
        expired_at = DateTime.Now.AddSeconds(seconds)
      };

      return entity;
    }

    public void Verify(int id, string code)
    {
      var ev = Table.SingleOrDefault(ev => ev.id == id);

      if (ev.code != code) {
        throw new FailureOperation("邮箱验证码错误");
      }

      ev.is_verified = true;
    }
  }
}
