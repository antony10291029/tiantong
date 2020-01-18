namespace Wcs.Plc.Test
{
  public class Plc : Wcs.Plc.Plc
  {
    protected override IPlcContainer ResolveContainer()
    {
      return new PlcContainer();
    }
  }
}
