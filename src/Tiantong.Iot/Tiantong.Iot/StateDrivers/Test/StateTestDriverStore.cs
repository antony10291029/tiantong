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
      try {
        return JsonSerializer.Deserialize<T>(_store[key]);
      } catch {
        _store[key] = JsonSerializer.Serialize(default(T));

        return default(T);
      }
    }
  }
}
