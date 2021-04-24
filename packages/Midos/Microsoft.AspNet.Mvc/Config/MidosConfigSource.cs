using Microsoft.Extensions.Configuration;

namespace Midos.Web.Config
{
  public class MidosConfigSource: IConfigurationSource
  {
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
      return new MidosConfigProvider();
    }
  }
}
