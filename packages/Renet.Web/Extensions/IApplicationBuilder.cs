using System;
using Microsoft.AspNetCore.Builder;

namespace Renet.Web
{
  public static class IApplicationBuilderExtensions
  {
    public static void UseProvider<T>(this IApplicationBuilder app) where T : IAppProvider
    {
      var provider = Activator.CreateInstance(typeof(T)) as IAppProvider;

      provider.Configure(app);
    }
  }
}
