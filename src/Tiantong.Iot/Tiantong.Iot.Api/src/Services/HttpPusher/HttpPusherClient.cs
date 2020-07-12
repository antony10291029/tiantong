using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class HttpPusherClient
  {
    private HttpClient _client = new HttpClient();

    public DomainContextFactory _domain;

    public HttpPusherClient(DomainContextFactory domain)
    {
      _domain = domain;
      _client.DefaultRequestHeaders
        .Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public void Timeout(int mileseconds)
    {
      _client.Timeout = new TimeSpan(0, 0, 0, 0, mileseconds);
    }

    public async Task PostAsync(int id, string uri, string header, string body, Encoding encoding = null)
    {
      using (var request = new HttpRequestMessage(HttpMethod.Post, uri)) {
        request.Content = new StringContent(body, encoding ?? Encoding.UTF8, "application/json");

        foreach (var keyValue in JsonSerializer.Deserialize<Dictionary<string, object>>(header)) {
          request.Headers.Add(keyValue.Key, keyValue.Value.ToString());
        }

        try {
          var response = await _client.SendAsync(request);

          var statusCode = ((int) response.StatusCode);
          var responseContent = await response.Content.ReadAsStringAsync();

          var obj = JsonSerializer.Deserialize<object>(responseContent);

          _domain.Log(new HttpPusherLog {
            pusher_id = id,
            request = body,
            response = responseContent,
            status_code = statusCode.ToString()
          });
        } catch (Exception e) {
          _domain.Log(new HttpPusherError {
            pusher_id = id,
            message = e.Message,
          });

          throw e;
        }
      }
    }
  }
}
