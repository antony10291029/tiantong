using System.Text;
using System;

namespace Tiantong.Iot
{
  public interface IWatcher<T>
  {
    IWatcher<T> Id(int plcId, int stateId, int watcherId);

    IWatcher<T> When(Func<T, bool> when);

    void Emit(T value);

    void On(Action<T> handler);

    void HttpPost(string url, string valueKey, bool toString = false, string json = "{}", Encoding encoding = null);
  }
}
