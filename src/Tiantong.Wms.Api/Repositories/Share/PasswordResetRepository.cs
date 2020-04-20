using System.Linq;
using Renet.Web;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Wms.Api
{
  public class PasswordResetRepository : Repository<PasswordReset, int>
  {
    private UserRepository _users;

    private EmailVerificationRepository _emailVerifications;

    public PasswordResetRepository(
      DbContext db,
      UserRepository users,
      EmailVerificationRepository emailVerifications
    ): base(db) {
      _users = users;
      _emailVerifications = emailVerifications;
    }

    public PasswordReset CreateByEmail(string email, int seconds)
    {
      var user = _users.EnsureGetByEmail(email);
      var entity = new PasswordReset {
        user_id = user.id,
        email_verification = _emailVerifications.New(email, seconds)
      };
      DbContext.Add(entity);

      return entity;
    }

    public PasswordReset EnsureGet(int id)
    {
      var pr = DbContext.Set<PasswordReset>()
        .SingleOrDefault(pr => pr.id == id);

      if (pr == null) {
        throw new FailureOperation("");
      }

      return pr;
    }

    public void SubmitByEmail(int id, string code, string password)
    {
      var pr = DbContext.Set<PasswordReset>()
        .Include(pr => pr.email_verification)
        .SingleOrDefault(pr => pr.id == id);

      if (code == null && !pr.email_verification.is_verified) {
        throw new FailureOperation("邮箱验证码未认证");
      }

      if (code != null && pr.email_verification.code != code) {
        throw new FailureOperation("验证码错误");
      }

      var user = _users.EnsureGet(pr.user_id);

      user.password = password;
      _users.EncodePassword(user);
      DbContext.Remove(pr);
    }
  }
}
