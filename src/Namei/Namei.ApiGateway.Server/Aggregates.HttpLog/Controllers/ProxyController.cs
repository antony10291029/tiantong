using AspNetCore.Proxy;
using AspNetCore.Proxy.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Namei.ApiGateway.Server
{
  [Controller]
  public class ProxyController: ControllerBase
  {
    private static readonly HttpProxyOptions _options = HttpProxyOptionsBuilder.Instance
      .WithHttpClientName("proxy")
      .WithBeforeSend(async (context, request) => {
        var tracker = context.RequestServices
          .GetService<HttpLogTracker>();

        await tracker.TrackSource(context);
        await tracker.TrackRequestAsync(request);
      })
      .WithAfterReceive(async (context, response) => {
        await context.RequestServices
          .GetService<HttpLogTracker>()
          .TrackResponseAsync(response);
      })
      .WithHandleFailure(async (context, exception) => {
        await context.RequestServices
          .GetService<HttpLogTracker>()
          .TrackExceptionAsync(exception);

          throw exception;
      })
      .Build();

    private readonly ILogger<ProxyController> _logger;

    private readonly ProxyTable _proxyTable;

    public ProxyController(
      ILogger<ProxyController> logger,
      ProxyTable proxyTable
    ) {
      _logger = logger;
      _proxyTable = proxyTable;
    }

    [HttpGet]
    [HttpPost]
    [HttpPut]
    [HttpPatch]
    [HttpDelete]
    [HttpHead]
    [HttpOptions]
    [Route("/{**rest}")]
    public Task Proxy()
    {
      var path = Request.Path;
      var proxyTable = _proxyTable.Get();

      if (!proxyTable.ContainsKey(path)) {
        throw KnownException.Error("Api not found", 404);
      }

      var proxyRecord = proxyTable[path];
      var queryString = Request.QueryString.Value;

      _logger.LogInformation("正在代理网络");

      return this.HttpProxyAsync($"{proxyRecord.EndpointFullPath}{queryString}", _options);
    }
  }
}
