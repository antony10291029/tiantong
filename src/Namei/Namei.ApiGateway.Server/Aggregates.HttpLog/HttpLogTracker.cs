using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Namei.ApiGateway.Server
{
  public class HttpLogTracker
  {
    private readonly HttpLog _log = new();

    private readonly HttpLogService _httpLogService;

    public HttpLogTracker(HttpLogService httpLogService)
    {
      _httpLogService = httpLogService;
    }

    public Task TrackSource(HttpContext context)
    {
      var request = context.Request;

      _log.SourcePath = context.Request.Path;
      _log.RequestMethod = request.Method.ToString();

      return Task.CompletedTask;
    }

    public async Task TrackRequestAsync(HttpRequestMessage request)
    {
      _log.RequestVersion = request.Version.ToString();
      _log.RequestUri = request.RequestUri.ToString();
      _log.RequestHeaders = request.Headers.Concat(request.Content.Headers)
        .ToDictionary(kv => kv.Key, kv => new StringValues(kv.Value.ToArray()));
      _log.RequestBody = await request.Content.ReadAsStringAsync();
      _log.RequestedAt = DateTime.Now;
    }

    public async Task TrackResponseAsync(HttpResponseMessage response)
    {
      _log.ResponseVersion = response.Version.ToString();
      _log.ResponseStatus = response.StatusCode.ToString();
      _log.ResponseHeaders = response.Headers.Concat(response.Content.Headers)
        .ToDictionary(kv => kv.Key, kv => new StringValues(kv.Value.ToArray()));
      _log.ResponseBody = await response.Content.ReadAsStringAsync();
      _log.ResponsedAt = DateTime.Now;

      _httpLogService.Add(_log);
    }

    public Task TrackExceptionAsync(Exception e)
    {
      _log.Exception = e;
      _log.ResponsedAt = DateTime.Now;

      _httpLogService.Add(_log);

      return Task.CompletedTask;
    }
  }
}
