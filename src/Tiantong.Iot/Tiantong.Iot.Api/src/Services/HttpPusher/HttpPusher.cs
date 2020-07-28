using System.Text;
using System.Text.Json;
using System.Collections.Generic;

namespace Tiantong.Iot.Api
{
  public class HttpPusher<T>: Watcher<T>
  {
    private HttpPusherClient _httpClient;

    public HttpPusher(HttpPusherClient httpClient)
    {
      _httpClient = httpClient;
    }

    public HttpPusher<T> Post(string url, string valueKey, bool toString, string header, string body, Encoding encoding = null)
    {
      if (valueKey == "") {
        valueKey = "value";
      }
      var data = JsonSerializer.Deserialize<Dictionary<string, object>>(body ?? "{}");
      _handler = async value => {
        if (toString) {
          data[valueKey] = value.ToString();
        } else {
          data[valueKey] = value;
        }

        try {
          await _httpClient.PostAsync(_id, url, header, JsonSerializer.Serialize(data), encoding);
        } catch {

        }
      };

      return this;
    }
  }
}
