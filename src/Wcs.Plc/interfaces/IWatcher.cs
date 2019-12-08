using System;

namespace Wcs.Plc
{
  public interface IWatcher<T>
  {
    IWatcher<T> When(Func<T, bool> comparer);

    void Handle(T value);

    void OnCancel(Action cancel);

    void Cancel();

    IWatcher<T> Event(string key);

    IWatcher<T> EventVoid(string key);

    IWatcher<T> Event<R>(string key, R payload);

    IWatcher<T> Event(string key, Func<T, T> handler);

    IWatcher<T> Event<R>(string key, Func<T, R> handler);
  }

  public interface IComparableWatcher<T> : IWatcher<T>
  {
    IWatcher<T> When(string opt, T value);
  }
}
