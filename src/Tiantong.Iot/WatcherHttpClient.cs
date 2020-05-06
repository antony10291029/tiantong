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

    private List<HttpWatcherLog> _logs = new List<HttpWatcherLog>();

    private List<HttpWatcherError> _errorLogs = new List<HttpWatcherError>();

    private readonly object _logLock = new object();

    private readonly object _errorLogLock = new object();

    public WatcherHttpClient(IotDbContext db, IntervalManager intervalManager)
    {
      _db = db;
      _client.DefaultRequestHeaders
        .Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

      var interval = new Interval();
      interval.SetTime(500);
      interval.SetHandler(HandleLogs);
      intervalManager.Add(interval);
    }

    public async Task PostAsync(int plcId, int stateId, int watcherId, string uri, string data, Encoding encoding = null)
    {
      var content = new StringContent(data, encoding ?? Encoding.UTF8, "application/json");
      try {
        var response = await _client.PostAsync(uri, content);
        var statusCode = response.StatusCode;
        var responseContent = await response.Content.ReadAsStringAsync();
        var log = new HttpWatcherLog {
          plc_id = plcId,
          state_id = stateId,
          watcher_id = watcherId,
          request = data,
          response = responseContent,
          status_code = response.StatusCode.ToString()
        };
        lock (_logLock) {
          _logs.Add(log);
        }

        Console.WriteLine($"success to send http watcher, uri: {uri}, request: {data}, response: {responseContent}, status: {statusCode}");
      } catch (Exception e) {
        var errorLog = new HttpWatcherError {
          plc_id = plcId,
          state_id = stateId,
          watcher_id = watcherId,
          error = e.Message,
          detail = e.Source,
        };
        lock (_errorLogLock) {
          _errorLogs.Add(errorLog);
        }
        Console.WriteLine($"fail to send http watcher, error: {e.Message}, source: {e.Source}");
        throw e;
      }
    }

    public void HandleLogs()
    {
      HttpWatcherLog[] logs;
      HttpWatcherError[] errorLogs;

      lock (_logLock) {
        logs = _logs.ToArray();
        _logs.Clear();
      }

      lock (_errorLogs) {
        errorLogs = _errorLogs.ToArray();
        _errorLogs.Clear();
      }

      _db.AddRange(logs);
      _db.AddRange(errorLogs);
      _db.SaveChanges();
    }

  }
}
