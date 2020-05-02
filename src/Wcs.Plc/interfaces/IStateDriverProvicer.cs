namespace Wcs.Plc
{
  public interface IStateDriverProvider
  {
    IStateDriver Resolve();

    void Boot();
  }
}
