namespace Tiantong.Iot
{
  public class StateTestDriver : IStateDriver
  {
    private StateTestDriverStore _store;

    private string Key;

    public StateTestDriver(StateTestDriverStore store)
    {
      _store = store;
    }

    public void UseBool()
    {

    }

    public void UseUInt16()
    {

    }

    public void UseInt32()
    {

    }

    public void UseString(int length)
    {

    }

    public void UseBytes(int length)
    {

    }

    public void UseAddress(string key)
    {
      Key = key;
    }

    public void SetBool(bool data)
    {
      _store.Set(Key, data);
    }

    public void SetUInt16(ushort data)
    {
      _store.Set(Key, data);
    }

    public void SetInt32(int data)
    {
      _store.Set(Key, data);
    }


    public void SetString(string data)
    {
      _store.Set(Key, data);
    }

    public void SetBytes(byte[] data)
    {
      _store.Set(Key, data);
    }

    public bool GetBool()
    {
      return _store.Get<bool>(Key);
    }

    public ushort GetUInt16()
    {
      return _store.Get<ushort>(Key);
    }

    public int GetInt32()
    {
      return _store.Get<int>(Key);
    }

    public string GetString()
    {
      return _store.Get<string>(Key);
    }

    public byte[] GetBytes()
    {
      return _store.Get<byte[]>(Key);
    }
  }
}
