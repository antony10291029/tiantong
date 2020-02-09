namespace Wcs.Plc.Test
{
  public class Plc : Wcs.Plc.Plc
  {
    public override DatabaseProvider ResolveDatabaseProvider()
    {
      return new TestDatabaseProvider();
    }

    /// EventLogger 将被单独测试
    public override EventPlugin ResolveEventLogger()
    {
      return null;
    }

    /// StateLogger 将被单独测试
    public override IStatePlugin ResolveStateLogger()
    {
      return null;
    }
  }
}
