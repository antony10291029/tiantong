using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Midos.Domain.Test
{

  [TestClass]
  public class PostgresOptionsTest
  {
    [ClassInitialize]
    public static void Initialize(TestContext ctx)
    {
      using var domain = new TestDomainContext();

      domain.Database.EnsureDeleted();
      domain.Database.EnsureCreated();
    }

    [ClassCleanup]
    public static void Cleanup()
    {
      using var domain = new TestDomainContext();

      domain.Database.EnsureDeleted();
    }
  }
}
