using System.Collections.Generic;

namespace Wcs.Plc
{
  public interface IStateManager
  {
    IStateBool Bool(string name);

    IStateUInt16 UInt16(string name);

    IStateInt32 Int32(string name);

    IStateString String(string name, int length);

    //

    IStateUInt16 UShort(string name);

    IStateInt32 Int(string name);

  }
}
