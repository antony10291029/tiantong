using Microsoft.VisualStudio.TestTools.UnitTesting;
using Namei.Wcs.Aggregates;

namespace Namei.Wcs.Api.Test
{
  public static class TestData
  {
    public readonly static LifterAgcTaskType LifterAgcTaskType
      = new(
        key: "test",
        name: "name",
        webHook: "webhook"
      );

    public readonly static AgcTaskType AgcTaskType
      = AgcTaskType.From(
        key: "test",
        name: "test.name",
        method: "wcs.move",
        webhook: "http://localhost",
        isEnabled: true
      );
  }

  [TestClass]
  public class Main
  {
    [AssemblyInitialize]
    public static void Up(TestContext _)
    {
      using var domain = Utils.GetDomain();

      domain.Database.EnsureDeleted();
      domain.Database.EnsureCreated();

      domain.Add(TestData.LifterAgcTaskType);
      domain.Add(TestData.AgcTaskType);
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
