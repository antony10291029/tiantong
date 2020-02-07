namespace Wcs.Plc.Test
{
  public class Plc : Wcs.Plc.Plc
  {
    protected override Wcs.Plc.PlcContainer ResolveContainer()
    {
      return new PlcContainer();
    }
  }
}
