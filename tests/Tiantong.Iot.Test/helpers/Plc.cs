namespace Tiantong.Iot.Test
{
  public class PlcWorker : Tiantong.Iot.PlcWorker
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
