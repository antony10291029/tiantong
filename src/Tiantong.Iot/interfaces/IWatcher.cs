using System.Text;
using System;

namespace Tiantong.Iot
{
  public interface IWatcher
  {
    IWatcher Id(int id);

    void On(Action<string> handler);

    void On(Action handler);

    IWatcher When(string opt, string value);

    void HttpPost(string url, string valueKey, bool toString = false, string json = "{}", Encoding encoding = null);
  }

  public interface IWatcher<T>: IWatcher
  {
    IWatcher<T> When(Func<T, bool> when);

    void Emit(T value);

    void On(Action<T> handler);

  }
}
