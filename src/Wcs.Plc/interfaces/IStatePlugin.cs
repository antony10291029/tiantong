namespace Wcs.Plc
{
  public interface IStatePlugin
  {
    void Install<T>(State<T> state);
  }
}
