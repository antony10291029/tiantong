using System;

namespace Wcs.Plc
{
  public interface IWatcher<T>
  {
    IWatcher<T> When(Func<T, bool> when);

    void Handle(T value);

    void OnCancel(Action cancel);

    void Cancel();

    void Event(string key);

    void EventVoid(string key);

    void Event<R>(string key, R payload);

    void Event(string key, Func<T, T> handler);

    void Event<R>(string key, Func<T, R> handler);
  }
}
