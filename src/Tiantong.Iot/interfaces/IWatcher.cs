using System;

namespace Tiantong.Iot
{
  public interface IWatcher<T>
  {
    IWatcher<T> When(Func<T, bool> when);

    void Emit(T value);

    void On(Action<T> handler);
  }
}
