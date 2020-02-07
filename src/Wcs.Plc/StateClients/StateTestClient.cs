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

    public Task SetWord(int data)
    {
      Store.Set(Key, data);

      return Task.Delay(0);
    }

    public Task SetWords(string data)
    {
      Store.Set(Key, data);

      return Task.Delay(0);
    }

    public Task SetBit(bool data)
    {
      Store.Set(Key, data);

      return Task.Delay(0);
    }

    public Task SetBits(string data)
    {
      Store.Set(Key, data);

      return Task.Delay(0);
    }

    public async Task<int> GetWord()
    {
      await Task.Delay(0);

      return Store.Get<int>(Key);
    }

    public async Task<string> GetWords()
    {
      await Task.Delay(0);

      return Store.Get<string>(Key);
    }

    public async Task<bool> GetBit()
    {
      await Task.Delay(0);

      return Store.Get<bool>(Key);
    }

    public async Task<string> GetBits()
    {
      await Task.Delay(0);

      return Store.Get<string>(Key);
    }
  }
}
