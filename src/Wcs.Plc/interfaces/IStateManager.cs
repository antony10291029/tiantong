namespace Wcs.Plc
{
  public interface IStateManager
  {
    IStateBool Bool(string key);

    IStateInt Int(string key);

    IStateString String(string key, int length = 1);
  }
}
