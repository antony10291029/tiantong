using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCore.Proxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Namei.ApiGateway.Server
{
  [Controller]
  public class AppController: ControllerBase
  {
    private readonly ProxyTable _proxyTable;

    private readonly ILogger<AppController> _logger;

    public AppController(
      ProxyTable proxyTable,
      ILogger<AppController> logger
    ) {
      _proxyTable = proxyTable;
      _logger = logger;
    }

    [HttpGet]
    [HttpPost]
    [HttpPut]
    [HttpPatch]
    [HttpDelete]
    [HttpHead]
    [HttpOptions]
    [Route("/{**rest}")]
    public Task Proxy(string rest)
    {
      var queryString = Request.QueryString.Value ?? "";
      var record = _proxyTable.Get()[$"/{rest}"];

      _logger.LogInformation(
        "正在将接口 {Path} 代理至 {Endpoint}/{EndpointPath}",
        record.Path, record.Endpoint, record.EndpointPath
      );

      return this.HttpProxyAsync($"{record.EndpointFullpath}{queryString}");
    }

    [HttpPost("/$proxy-table/get")]
    public IReadOnlyDictionary<string, ProxyRecord> GetMap()
      => _proxyTable.Get();

    [HttpPost("/$proxy-table/fetch")]
    public IReadOnlyDictionary<string, ProxyRecord> Fetch()
      => _proxyTable.Fetch();

    [HttpPost("/$proxy-table/sync")]
    public INotifyResult<IMessageObject> Sync()
    {
      _proxyTable.Sync();

      return NotifyResult.FromVoid().Success("映射表已更新");
    }
  }
}
