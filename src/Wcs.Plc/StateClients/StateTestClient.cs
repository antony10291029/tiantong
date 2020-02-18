using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateTestClient : IStateClient
  {
    public StateTestClientStore Store;

    private string Key;

    private int Length;

    public void SetKey(string key)
    {
      Key = key;
    }

    public void SetLength(int length)
    {
      Length = length;
    }

    public void SetInt(int data)
    {
      Store.Set(Key, data);
    }

    public void SetString(string data)
    {
      Store.Set(Key, data);
    }

    public void SetBool(bool data)
    {
      Store.Set(Key, data);
    }

    public int GetInt()
    {
      return Store.Get<int>(Key);
    }

    public string GetString()
    {
      return Store.Get<string>(Key);
    }

    public bool GetBool()
    {
      return Store.Get<bool>(Key);
    }
  }
}
