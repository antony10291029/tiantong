using System.Text;
using System.Text.Json;
using System.Collections.Generic;

namespace Tiantong.Iot.Api
{
  public class HttpPusher<T>: Watcher<T>
  {
    private HttpPusherClient _httpClient;

    private bool _isConcurrent = true;

    private bool _isWaiting = false;

    public HttpPusher(HttpPusherClient httpClient)
    {
      _httpClient = httpClient;
    }

    public HttpPusher<T> IsConcurrent(bool value)
    {
      _isConcurrent = value;

      return this;
    }

    public HttpPusher<T> Post(string url, string valueKey, bool toString = false, string header = null, string body = null, Encoding encoding = null)
    {
      if (valueKey == "") {
        valueKey = "value";
      }
      var data = JsonSerializer.Deserialize<Dictionary<string, object>>(body ?? "{}");
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
          await _httpClient.PostAsync(_id, url, header, JsonSerializer.Serialize(data), encoding);
        } catch {

        } finally {
          _isWaiting = false;
        }
      };

      return this;
    }
  }
}
