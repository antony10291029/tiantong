using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Midos.Domain;
using System;

namespace Midos.Controllers
{
  [Controller]
  [Route("/$seeder")]
  public abstract class SeederController
  {
    private readonly DomainContext _context;

    private readonly ILogger<SeederController> _logger;

    public SeederController(
      DomainContext context,
      ILogger<SeederController> logger,
      IHostEnvironment env
    ) {
      _context = context;
      _logger = logger;
      if (!env.IsDevelopment()) {
        throw new InvalidOperationException("无法在非开发环境中使用该功能");
      }
    }

    protected abstract void Seed();

    [HttpPost("insert")]
    public INotifyResult<IMessageObject> Insert()
    {
      var msg = "测试数据已插入";

      _context.Database.EnsureCreated();
      Seed();
      _logger.LogInformation(msg);

      return NotifyResult.FromVoid().Success(msg);
    }

    [HttpPost("refresh")]
    public INotifyResult<IMessageObject> Refresh()
    {
      var msg = "数据已重新插入";

      _context.Database.EnsureDeleted();
      _context.Database.EnsureCreated();
      Seed();
      _logger.LogInformation(msg);

      return NotifyResult.FromVoid().Success(msg);
    }
  }
}
