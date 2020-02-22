namespace Wcs.Plc.Test
{
  public class Plc : Wcs.Plc.Plc
  {
    public override void ResolveDatabaseProvider()
    {
      DatabaseProvider = new TestDatabaseProvider();
    }

    /// EventLogger 将被单独测试
    public override void ResolveEventLogger()
    {

    }

    /// StateLogger 将被单独测试
    public override void ResolveStateLogger()
    {

    }
  }
}
