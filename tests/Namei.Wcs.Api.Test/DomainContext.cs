using Midos.Domain.Test;

namespace Namei.Wcs.Api.Test
{
  public class Utils
  {
    public static DomainContext GetDomain()
      => new TestDomainContext();
  }

  class TestDomainContext: DomainContext
  {
    public TestDomainContext(): base(
      new PostgresDomainOptions<DomainContext>(),
      new TestEventPublisher()
    ) {}
  }
}
