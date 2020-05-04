using System.Collections.Generic;
using System;
using System.Text.Json;

namespace Tiantong.Iot
{
  public class Watcher<T> : IWatcher<T>
  {
    private IWatcherHttpClient _httpClient;

    private Action _cancel;

    private Action<T> _handler;

    protected Func<T, bool> _when;

    public Watcher(IWatcherHttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    public void Emit(T value)
    {
      if (_when(value)) {
        try {
          _handler(value);
        } catch (Exception e) {
          Console.WriteLine(e);
        }
      }
    }

    public IWatcher<T> When(Func<T, bool> when)
    {
      _when = when;

      return this;
    }

    public void On(Action<T> handler)
    {
      _handler = handler;
    }

    public void HttpPost(string url, string valueKey, bool toString = false, string json = null)
    {
      var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json ?? "{}");
      _handler = (T value) => {
        if (toString) {
          data[valueKey] = value.ToString();
        } else {
          data[valueKey] = value;
        }

        _httpClient.Post(url, JsonSerializer.Serialize(data));
      };
    }

    public void OnCancel(Action cancel)
    {
      _cancel = cancel;
    }

  }
}
