using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Midos.SeedWork.Domain;

namespace Namei.ApiGateway.Server
{
  public class AppMigrationController: MigrationController
  {
    public AppMigrationController(
      AppContext context,
      IHostEnvironment env,
      ILogger<AppMigrationController> logger
    ): base(context, env, logger) {}
  }
}
