using System.Net;
using System;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public class WatcherHttpClient: IWatcherHttpClient
  {
    private HttpClient _client = new HttpClient();

    private readonly object _sendLock = new object();

    private IotDbContext _db;

    public HttpPusherLogger _logger;

    public WatcherHttpClient(IotDbContext db, IntervalManager intervalManager)
    {
      _db = db;
      _client.DefaultRequestHeaders
        .Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      _logger = new HttpPusherLogger(db, intervalManager);
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
        var statusCode = response.StatusCode;
        var responseContent = await response.Content.ReadAsStringAsync();
        _logger.Log(new HttpPusherLog {
          pusher_id = id,
          request = data,
          response = responseContent,
          status_code = response.StatusCode.ToString()
        });

        Console.WriteLine($"success to send http watcher, uri: {uri}, request: {data}, response: {responseContent}, status: {statusCode}");
      } catch (Exception e) {
        _logger.LogError(new HttpPusherError {
          pusher_id = id,
          error = e.Message,
          detail = e.Source,
        });

        Console.WriteLine($"fail to send http watcher, error: {e.Message}, source: {e.Source}");
        throw e;
      }
    }

  }
}