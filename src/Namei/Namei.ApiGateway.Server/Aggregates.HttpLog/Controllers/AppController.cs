using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Namei.ApiGateway.Server
{
  [Controller]
  public class AppController
  {
    private readonly AppConfig _config;

    private readonly ProxyTable _proxyTable;

    private readonly ILogger<AppController> _logger;

    public AppController(
      AppConfig config,
      ProxyTable proxyTable,
      ILogger<AppController> logger
    ) {
      _config = config;
      _proxyTable = proxyTable;
      _logger = logger;
    }

    [HttpPost("/$proxy-table/env")]
    public object GetConfig() => new { Env = _config.ApiEnv };

    [HttpPost("/$proxy-table/get")]
    public IReadOnlyDictionary<string, ProxyRecord> GetMap()
      => _proxyTable.Get();

    [HttpPost("/$proxy-table/fetch")]
    public IReadOnlyDictionary<string, ProxyRecord> Fetch()
      => _proxyTable.Fetch();

    [HttpPost("/$proxy-table/sync")]
    public INotifyResult<IMessageObject> Sync()
    {
      var msg = "代理映射表已更新";

      _proxyTable.Sync();
      _logger.LogInformation(msg);

      return NotifyResult.FromVoid().Success(msg);
    }
  }
}
