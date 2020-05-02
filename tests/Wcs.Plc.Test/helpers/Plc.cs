namespace Wcs.Plc.Test
{
  public class Plc : Wcs.Plc.Plc
  {
    public override void ResolveDatabaseProvider()
    {
      DatabaseProvider = new TestDatabaseProvider();
    }

    /// StateLogger 将被单独测试
    public override void ResolveStateLogger()
    {

    }
  }
}
