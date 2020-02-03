using Microsoft.AspNetCore.Builder;

namespace Renet.Web
{
  public interface IAppProvider
  {
    void Configure(IApplicationBuilder app);
  }
}
