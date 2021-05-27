using Midos.Domain.Test;
using Namei.Wcs.Aggregates;

namespace Namei.Wcs.Api.Test
{
  public class Utils
  {
    public static WcsContext GetDomain()
      => new TestDomainContext();

    public static RcsContext GetRcsContext()
      => new TestRcsContext();
  }

  class TestDomainContext: WcsContext
  {
    public TestDomainContext(): base(
      new PostgresDomainOptions<DomainContext>("wcs.test"),
      new TestEventPublisher()
    ) {}
  }

  class TestRcsContext: RcsContext
  {
    public TestRcsContext(): base(
      new PostgresDomainOptions<RcsContext>("rcs.test"),
      new TestEventPublisher()
    ) {}
  }
}
