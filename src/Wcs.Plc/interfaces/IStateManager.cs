using System.Collections.Generic;

namespace Wcs.Plc
{
  public interface IStateManager
  {
    IStateBool Bool(string key);

    IStateUInt16 UInt16(string key);

    IStateInt32 Int32(string key);

    IStateString String(string key, int length);

    //

    IStateUInt16 UShort(string key);

    IStateInt32 Int(string key);

  }
}
