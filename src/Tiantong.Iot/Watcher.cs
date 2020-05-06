using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;

namespace Tiantong.Iot
{
  public abstract class Watcher: IWatcher
  {
    protected int _plcId;

    protected int _stateId;

    protected int _watcherId;

    public IWatcher Id(int plcId, int stateId, int watcherId)
    {
      _plcId = plcId;
      _stateId = stateId;
      _watcherId = watcherId;

      return this;
    }

    public abstract void On(Action handler);

    public abstract void On(Action<string> handler);

    public abstract IWatcher When(string opt, string value);

    public abstract void HttpPost(string url, string valueKey, bool toString, string json, Encoding encoding);
  }

  public class Watcher<T> : Watcher, IWatcher<T>
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

    public override IWatcher When(string opt, string value)
    {
      _when = opt switch {
        ""   => _ => true,
        "="  => data => data.ToString() == value,
        "==" => data => data.ToString() == value,
        "!=" => data => data.ToString() != value,
        "<>" => data => data.ToString() != value,
        ">"  => data => double.Parse(data.ToString()) > double.Parse(value),
        "<"  => data => double.Parse(data.ToString()) < double.Parse(value),
        ">=" => data => double.Parse(data.ToString()) >= double.Parse(value),
        "<=" => data => double.Parse(data.ToString()) <= double.Parse(value),
        _  => throw new Exception("暂时不支持该操作符")
      };

      return this;
    }

    public void On(Action<T> handler)
    {
      _handler = handler;
    }

    public override void On(Action handler)
    {
      On(_ => handler());
    }

    public override void On(Action<string> handler)
    {
      On(data => handler(data.ToString()));
    }

    public override void HttpPost(string url, string valueKey, bool toString = false, string json = null, Encoding encoding = null)
    {
      var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json ?? "{}");
      _handler = value => {
        if (toString) {
          data[valueKey] = value.ToString();
        } else {
          data[valueKey] = value;
        }

        _httpClient.PostAsync(_plcId, _stateId, _watcherId, url, JsonSerializer.Serialize(data), encoding);
      };
    }

    public void OnCancel(Action cancel)
    {
      _cancel = cancel;
    }

  }
}
