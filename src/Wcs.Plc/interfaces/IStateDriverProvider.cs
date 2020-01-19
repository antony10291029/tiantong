namespace Wcs.Plc
{
  public interface IStateDriverProvider
  {
    IStateDriver Resolve();
  }
}
