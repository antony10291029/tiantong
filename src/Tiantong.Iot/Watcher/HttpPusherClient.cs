using System;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public class HttpPusherClient: IHttpPusherClient
  {
    private HttpClient _client = new HttpClient();

    public HttpPusherLogger _logger;

    public HttpPusherClient(HttpPusherLogger logger)
    {
      _logger = logger;
      _client.DefaultRequestHeaders
        .Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public void Timeout(int mileseconds)
    {
      _client.Timeout = new TimeSpan(0, 0, 0, 0, mileseconds);
    }

    public async Task PostAsync(int id, string uri, string data, Encoding encoding = null)
    {
      var content = new StringContent(data, encoding ?? Encoding.UTF8, "application/json");
      try {
        var response = await _client.PostAsync(uri, content);
        var statusCode = ((int) response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        _logger.Log(new HttpPusherLog {
          pusher_id = id,
          request = data,
          response = responseContent,
          status_code = statusCode.ToString()
        });
      } catch (Exception e) {
        _logger.LogError(new HttpPusherError {
          pusher_id = id,
          message = e.Message,
        });

        throw e;
      }
    }

  }

}
