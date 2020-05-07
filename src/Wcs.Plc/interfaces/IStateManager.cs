using System.Collections.Generic;

namespace Wcs.Plc
{
  public interface IStateManager
  {
    IStateBuilder<bool> Bool(string name);

    IStateBuilder<ushort> UInt16(string name);

    IStateBuilder<int> Int32(string name);

    IStateBuilder<string> String(string name, int length);

    //

    IStateBuilder<ushort> UShort(string name);

    IStateBuilder<int> Int(string name);

  }
}
