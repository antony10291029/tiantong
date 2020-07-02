using Renet.Utils;
using Renet.Web;
using System;
using System.Linq;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class SystemRepository
  {
    private IHash _hash;

    private IRandom _random;

    private LogContext _log;

    private SystemContext _system;

    public SystemRepository(IHash hash, IRandom random, LogContext log, SystemContext system)
    {
      _log = log;
      _hash = hash;
      _random = random;
      _system = system;
    }

    public bool IsMigrated()
    {
      return _system.HasTable("key_values");
    }

    public bool HasPassword()
    {
      return _system.KeyValues.FirstOrDefault(kv => kv.key == "admin_password") != null;
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

      _system.Add(new KeyValue {
        key = "admin_password",
        value = _hash.Make(password)
      });
      _system.SaveChanges();
    }

    public void UnsetAdminPassword()
    {
      var keyValue = _system.KeyValues.FirstOrDefault(kv => kv.key == "admin_password");

      _system.Remove(keyValue);
      _system.SaveChanges();
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

      var keyValue = _system.KeyValues.FirstOrDefault(kv => kv.key == "admin_password");

      if (!_hash.Match(oldPassword, keyValue.value)) {
        throw new FailureOperation("修改失败，原始密码错误");
      }

      keyValue.value = _hash.Make(password);

      _system.SaveChanges();
    }

    public void EnsureAdminParams(string password)
    {
      var keyValue = _system.KeyValues.FirstOrDefault(kv => kv.key == "admin_password");

      if (keyValue == null || !_hash.Match(password, keyValue.value)) {
        throw new FailureOperation("密码确认失败");
      }
    }

    public EmailVerifyCode CreateEmailVerifyCode(string email)
    {
      var ev = new EmailVerifyCode {
        email = email,
        verify_code = _random.Int(100000, 999999).ToString()
      };

      _log.EmailVerifyCodes.Add(ev);

      _log.SaveChanges();

      return ev;
    }

    public void EnsureEmailVerifyCode(int id, string email, string code)
    {
      var verify = _log.EmailVerifyCodes.FirstOrDefault(ev => ev.id == id && ev.email == email);

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

      _system.KeyValues.Add(keyValue);
      _system.SaveChanges();
    }

    public void UnsetAdminEmail()
    {
      var keyValue = _system.KeyValues.First(kv => kv.key == "admin_email");

      _system.Remove(keyValue);
      _system.SaveChanges();
    }

    public string GetAdminEmail()
    {
      var keyValue = _system.KeyValues.FirstOrDefault(kv => kv.key == "admin_email");

      return keyValue?.value;
    }

    public void EnsureSystemUnlocked()
    {
      var keyValue = _system.KeyValues.FirstOrDefault(kv => kv.key == "is_system_locked");

      if (keyValue != null && keyValue.value == "true") {
        throw new FailureOperation("系统设备锁定已打开，无法执行当前操作");
      }
    }

    public void LockSystem()
    {
      var keyValue = _system.KeyValues.FirstOrDefault(kv => kv.key == "is_system_locked");

      if (keyValue == null) {
        keyValue = new KeyValue {
          key = "is_system_locked"
        };
        _system.Add(keyValue);
      }

      keyValue.value = "true";
      _system.SaveChanges();
    }

    public void UnlockSystem()
    {
      var keyValue = _system.KeyValues.FirstOrDefault(kv => kv.key == "is_system_locked");

      if (keyValue == null) {
        keyValue = new KeyValue {
          key = "is_system_locked"
        };
        _system.Add(keyValue);
      }

      keyValue.value = "false";
      _system.SaveChanges();
    }

    public bool GetIsSystemLocked()
    {
      var keyValue = _system.KeyValues.FirstOrDefault(kv => kv.key == "is_system_locked");

      return keyValue?.value == "true";
    }

    public bool GetIsAutorun()
    {
      var keyValue = _system.KeyValues.FirstOrDefault(kv => kv.key == "is_autorun");

      return keyValue?.value == "true";
    }

    public void SetAutorun(bool value)
    {
      var keyValue = _system.KeyValues.FirstOrDefault(kv => kv.key == "is_autorun");

      if (keyValue == null) {
        keyValue = new KeyValue {
          key = "is_autorun",
        };

        _system.Add(keyValue);
      }

      keyValue.value = value ? "true" : "false";
      _system.SaveChanges();
    }

  }

}