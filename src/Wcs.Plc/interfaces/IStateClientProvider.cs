namespace Wcs.Plc
{
  public interface IStateClientProvider
  {
    IStateClient Resolve();
  }
}
