using System;

namespace Wcs.Plc
{
  public interface IWatcher<T>
  {
    void Emit(T value);

    void On(Action<T> handler);
  }
}
