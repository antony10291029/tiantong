using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Test
{
  public class PlcWorker : Tiantong.Iot.PlcWorker
  {
    public PlcWorker()
    {
      Model(PlcModel.Test);
    }

    public override IPlcWorkerLogger ResolvePlcWorkerLogger()
    {
      return new TestPlcWorkerLogger();
    }

    public override DatabaseManager ResolveDatabaseManager()
    {
      return new TestDatabaseManager();
    }

    public override IHttpPusherClient ResolveHttpPusherClient()
    {
      return new TestHttpPusherClient();
    }

    /// StateLogger 将被单独测试

    public override IStateLogger ResolveStateLogger()
    {
      return null;
    }
  }
}
