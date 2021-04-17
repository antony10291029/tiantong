using Midos.Domain.Test;

namespace Namei.Wcs.Api.Test
{
  public class Utils
  {
    public static WcsContext GetDomain()
      => new TestDomainContext();
  }

  class TestDomainContext: WcsContext
  {
    public TestDomainContext(): base(
      new PostgresDomainOptions<DomainContext>(),
      new TestEventPublisher()
    ) {}
  }
}
