using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Namei.ApiGateway.Server
{
  [Controller]
  [Route("/$database")]
  public class DevController
  {
    private readonly DatabaseContext _context;

    private readonly ILogger<DevController> _logger;

    public DevController(
      DatabaseContext context,
      IHostEnvironment env,
      ILogger<DevController> logger
    ) {
      if (!env.IsDevelopment()) {
        throw new InvalidOperationException("无法在非开发环境中使用改功能");
      }

      _context = context;
      _logger = logger;
    }

    public static class LogEvent
    {
      public static readonly EventId DatabaseCreated = new(0, nameof(DatabaseCreated));

      public static readonly EventId DatabaseDeleted = new(0, nameof(DatabaseDeleted));

      public static readonly EventId DatabaseRefreshed = new(0, nameof(DatabaseRefreshed));
    }

    [HttpPost("create")]
    public INotifyResult<IMessageObject> Migrate()
    {
      _context.Database.EnsureCreated();
      _logger.LogInformation(LogEvent.DatabaseCreated, "数据库已删除");

      return NotifyResult.FromVoid().Success("数据库已创建");
    }

    [HttpPost("drop")]
    public INotifyResult<IMessageObject> Drop()
    {
      _context.Database.EnsureDeleted();
      _logger.LogInformation(LogEvent.DatabaseDeleted, "数据库已删除");

      return NotifyResult.FromVoid().Success("数据库已删除");
    }

    [HttpPost("refresh")]
    public INotifyResult<IMessageObject> Refresh()
    {
      _context.Database.EnsureDeleted();
      _context.Database.EnsureCreated();
      _logger.LogInformation(LogEvent.DatabaseRefreshed, "数据库已删除");

      return NotifyResult.FromVoid().Success("数据库已刷新");
    }
  }
}
