namespace Wcs.Plc
{
  public interface IStateManager
  {
    IStateBool Bool(string key, int length = 1);

    IStateInt Int(string key, int length = 4);

    IStateString String(string key, int length = 10);
  }
}
