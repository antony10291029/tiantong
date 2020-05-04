using System.Text.Json;
using System.Collections.Generic;

namespace Tiantong.Iot
{
  public class StateTestDriverStore
  {
    private Dictionary<string, string> _store = new Dictionary<string, string>();

    public void Set<T>(string key, T value)
    {
      var data = JsonSerializer.Serialize(value);

      _store[key] = data;
    }

    public T Get<T>(string key)
    {
      string data;
      T value = default(T);

      try {
        data = _store[key];
        value = JsonSerializer.Deserialize<T>(data);
      } catch {}

      return value;
    }
  }
}
