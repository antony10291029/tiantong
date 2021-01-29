using Microsoft.AspNetCore.Builder;

namespace Renet.Web
{
  public abstract class AppProvider : IAppProvider
  {
    public abstract void Configure(IApplicationBuilder app);
  }
}
