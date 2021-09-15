using Midos.Utils;
using System.Linq;
using Tiantong.Iot.Entities;
using Microsoft.EntityFrameworkCore;

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
