using System.Threading.Tasks;
using System;
using System.Collections.Generic;
namespace Tiantong.Iot.Plc
{
  public class StateWorker
  {

  }

  public abstract class StateWorker<T>: StateWorker
  {
    protected IStateDriver _driver;

    private List<Func<T, Task>> _getHooks;

    private List<Func<T, Task>> _setHooks;

    public T CurrentValue = default;

    public StateWorker(IStateDriver driver)
    {
      _driver = driver;
    }

    public void Set(T data)
    {
      var tasks = new List<Task>();
      HandleSet(data);

      foreach (var hook in _setHooks) {
        hook(data);
      }
    }

    public T Get()
    {
      var tasks = new List<Task>();
      var data = CurrentValue = HandleGet();

      foreach (var hook in _getHooks) {
        hook(data);
      }

      return data;
    }

    protected abstract T HandleGet();

    protected abstract void HandleSet(T data);

  }
}
