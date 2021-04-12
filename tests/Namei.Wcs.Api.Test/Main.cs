using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Namei.Wcs.Api.Test
{
  [TestClass]
  public class Main
  {
    [AssemblyInitialize]
    public static void Up(TestContext context)
    {
      using var domain = Utils.GetDomain();
      domain.Database.EnsureCreated();
    }

    [AssemblyCleanup]
    public static void Down()
    {
      using var domain = Utils.GetDomain();

      domain.Database.EnsureDeleted();
    }
  }
}
