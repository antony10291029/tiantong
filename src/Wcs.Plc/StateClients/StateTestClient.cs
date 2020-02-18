using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateTestClient : IStateClient
  {
    public StateTestClientStore Store;

    private string Key;

    private int Length;

    public IStateClient SetKey(string key)
    {
      Key = key;

      return this;
    }

    public IStateClient SetLength(int length)
    {
      Length = length;

      return this;
    }

    public Task SetInt(int data)
    {
      Store.Set(Key, data);

      return Task.Delay(0);
    }

    public Task SetString(string data)
    {
      Store.Set(Key, data);

      return Task.Delay(0);
    }

    public Task SetBool(bool data)
    {
      Store.Set(Key, data);

      return Task.Delay(0);
    }

    public async Task<int> GetInt()
    {
      await Task.Delay(0);

      return Store.Get<int>(Key);
    }

    public async Task<string> GetString()
    {
      await Task.Delay(0);

      return Store.Get<string>(Key);
    }

    public async Task<bool> GetBool()
    {
      await Task.Delay(0);

      return Store.Get<bool>(Key);
    }
  }
}
