using System.Text;
using System.Text.Json;
using System.Collections.Generic;

namespace Tiantong.Iot
{

  public class StateHttpPusher<T>: Watcher<T>, IStateHttpPusher
  {
    private IHttpPusherClient _httpClient;

    private bool _isConcurrent = true;

    private bool _isWaiting = false;

    public StateHttpPusher(IHttpPusherClient httpClient)
    {
      _httpClient = httpClient;
    }

    public IStateHttpPusher IsConcurrent(bool value)
    {
      _isConcurrent = value;

      return this;
    }

    public IStateHttpPusher Post(string url, string valueKey, bool toString = false, string json = null, Encoding encoding = null)
    {
      if (valueKey == "") {
        valueKey = "value";
      }
      var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json ?? "{}");
      _handler = async value => {
        if (!_isConcurrent && _isWaiting) {
          return;
        } else {
          _isWaiting = true;
        }

        if (toString) {
          data[valueKey] = value.ToString();
        } else {
          data[valueKey] = value;
        }

        try {
          await _httpClient.PostAsync(_id, url, JsonSerializer.Serialize(data), encoding);
        } catch {

        } finally {
          _isWaiting = false;
        }
      };

      return this;
    }
  }
}
