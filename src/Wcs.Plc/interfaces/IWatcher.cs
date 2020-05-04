using System;

namespace Wcs.Plc
{
  public interface IWatcher<T>
  {
    IWatcher<T> When(Func<T, bool> when);

    void Emit(T value);

    void On(Action<T> handler);
  }
}
