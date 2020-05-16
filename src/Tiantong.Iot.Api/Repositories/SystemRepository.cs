using System;
using System.Linq;
using Tiantong.Iot.Entities;
using Renet;
using Renet.Web;

namespace Tiantong.Iot.Api
{
  public class SystemRepository
  {
    private IHash _hash;

    private IRandom _random;

    private IotDbContext _db;

    public SystemRepository(IotDbContext db, IHash hash, IRandom random)
    {
      _db = db;
      _hash = hash;
      _random = random;
    }

    public bool HasPassword()
    {
      return _db.KeyValues.FirstOrDefault(kv => kv.key == "password") != null;
    }

    public void SetPassword(string password, string passwordConfirmation)
    {
      if (password.Length < 6) {
        throw new FailureOperation("密码长度不得少于6位");
      }

      if (password != passwordConfirmation) {
        throw new FailureOperation("密码输入不一致");
      }

      if (HasPassword()) {
        throw new FailureOperation("密码已设置，不可重新设置");
      }

      _db.Add(new KeyValue {
        key = "password",
        value = _hash.Make(password)
      });
      _db.SaveChanges();
    }

    public void ResetPassword(string oldPassword, string password, string passwordConfirmation)
    {
      if (password.Length < 6) {
        throw new FailureOperation("密码长度不得少于6位");
      }

      if (password != passwordConfirmation) {
        throw new FailureOperation("密码输入不一致");
      }

      if (!HasPassword()) {
        throw new FailureOperation("系统暂时无密码，请先设置");
      }

      var keyValue = _db.KeyValues.FirstOrDefault(kv => kv.key == "password");

      if (!_hash.Match(oldPassword, keyValue.value)) {
        throw new FailureOperation("修改失败，原始密码错误");
      }

      keyValue.value = password;
      _db.SaveChanges();
    }

    public void UnsetAdminPassword()
    {
      var keyValue = _db.KeyValues.FirstOrDefault(kv => kv.key == "password");

      _db.Remove(keyValue);
      _db.SaveChanges();
    }

    public EmailVerifyCode CreateEmailVerifyCode(string email)
    {
      var ev = new EmailVerifyCode {
        email = email,
        verify_code = _random.Int(100000, 999999).ToString()
      };

      _db.EmailVerifyCode.Add(ev);

      _db.SaveChanges();

      return ev;
    }

    public void EnsureEmailVerifyCode(int id, string email, string code)
    {
      var verify = _db.EmailVerifyCode.FirstOrDefault(ev => ev.id == id && ev.email == email);

      if (verify == null || verify.expired_at < DateTime.Now || verify.verify_code != code) {
        throw new FailureOperation("邮箱验证码错误");
      }
    }

    public void SetAdminEmail(string email)
    {
      var keyValue = new KeyValue {
        key = "admin_email",
        value = email
      };

      _db.KeyValues.Add(keyValue);
      _db.SaveChanges();
    }

    public void UnsetAdminEmail()
    {
      var keyValue = _db.KeyValues.First(kv => kv.key == "admin_email");

      _db.Remove(keyValue);
      _db.SaveChanges();
    }

    public string GetAdminEmail()
    {
      var keyValue = _db.KeyValues.FirstOrDefault(kv => kv.key == "admin_email");

      return keyValue?.value;
    }

  }

}
