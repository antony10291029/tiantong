using System;

namespace Tiantong.Iot
{
  public interface IStateManager
  {
    IStateManager Id(int id);

    IStateManager Name(string name);

    void Bool(string name, Action<IState<bool>> builder = null);

    void UInt16(string name, Action<IState<ushort>> builder = null);

    void Int32(string name, Action<IState<int>> builder = null);

    void String(string name, int length, Action<IState<string>> builder = null);

    //

    void UShort(string name, Action<IState<ushort>> builder = null);

    void Int(string name, Action<IState<int>> builder = null);

  }

}
