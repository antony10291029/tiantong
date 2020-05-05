using System;

namespace Tiantong.Iot
{
  public interface IStateManager
  {
    IStateManager Id(int id);

    IStateManager Name(string name);

    void Bool(string name, Action<IState<bool>> builder);

    void UInt16(string name, Action<IState<ushort>> builder);

    void Int32(string name, Action<IState<int>> builder);

    void String(string name, int length, Action<IState<string>> builder);

    //

    void UShort(string name, Action<IState<ushort>> builder);

    void Int(string name, Action<IState<int>> builder);

  }
}
