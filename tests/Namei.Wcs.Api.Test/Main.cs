using Microsoft.VisualStudio.TestTools.UnitTesting;
using Namei.Wcs.Aggregates;

namespace Namei.Wcs.Api.Test
{
  public static class TestData
  {
    public static LifterAgcTaskType LifterAgcTaskType
      = new LifterAgcTaskType(
        key: "test",
        name: "name",
        webHook: "webhook"
      );
  }

  [TestClass]
  public class Main
  {
    [AssemblyInitialize]
    public static void Up(TestContext context)
    {
      using var domain = Utils.GetDomain();

      domain.Database.EnsureCreated();

      domain.Add(TestData.LifterAgcTaskType);
      domain.SaveChanges();
    }

    [AssemblyCleanup]
    public static void Down()
    {
      using var domain = Utils.GetDomain();

      domain.Database.EnsureDeleted();
    }
  }
}
