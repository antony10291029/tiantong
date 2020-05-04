using System.Collections.Generic;

namespace Tiantong.Iot
{
  public interface IStateManager
  {
    IStateManager Id(int id);

    IStateManager Name(string name);

    IState<bool> Bool(string name);

    IState<ushort> UInt16(string name);

    IState<int> Int32(string name);

    IState<string> String(string name, int length);

    //

    IState<ushort> UShort(string name);

    IState<int> Int(string name);

  }
}
