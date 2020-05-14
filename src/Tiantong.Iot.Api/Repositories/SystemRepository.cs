using System.Linq;
using Tiantong.Iot.Entities;
using Renet;
using Renet.Web;

namespace Tiantong.Iot.Api
{
  public class SystemRepository
  {
    private IHash _hash;

    private IotDbContext _db;

    public SystemRepository(IotDbContext db, IHash hash)
    {
      _db = db;
      _hash = hash;
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
  }
}
