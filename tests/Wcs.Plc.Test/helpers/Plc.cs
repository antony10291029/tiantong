namespace Wcs.Plc.Test
{
  public class PlcWorker : Wcs.Plc.PlcWorker
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
