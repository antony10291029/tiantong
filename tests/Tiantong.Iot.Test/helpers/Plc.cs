namespace Tiantong.Iot.Test
{
  public class PlcWorker : Tiantong.Iot.PlcWorker
  {
    public override DatabaseProvider ResolveDatabaseProvider()
    {
      return new TestDatabaseProvider();
    }

    public override IWatcherHttpClient ResolveWatcherHttpClient()
    {
      return new TestWatcherHttpClient();
    }

    /// StateLogger 将被单独测试

    public override IStatePlugin ResolveStateLogger()
    {
      return null;
    }
  }
}
