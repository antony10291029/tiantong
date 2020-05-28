using System.Linq;
using System.Collections.Generic;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class KeyValues
  {
    private SystemContext _db;

    private Dictionary<string, string> _keyValues;

    public string Password
    {
      get => GetValue("password");
    }

    public KeyValues(SystemContext db)
    {
      _db = db;
    }

    private string GetValue(string key)
    {
      if (_keyValues == null) {
        _keyValues = _db.KeyValues.ToDictionary(
          kv => kv.key, kv => kv.value
        );
      }

      return _keyValues.ContainsKey(key) ? _keyValues[key] : null;
    }

  }

}
