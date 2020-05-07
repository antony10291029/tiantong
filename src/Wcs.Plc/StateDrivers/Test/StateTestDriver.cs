namespace Wcs.Plc
{
  public class StateTestDriver : IStateDriver
  {
    public StateTestDriverStore Store;

    private string Key;

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
      Store.Set(Key, data);
    }

    public void SetUInt16(ushort data)
    {
      Store.Set(Key, data);
    }

    public void SetInt32(int data)
    {
      Store.Set(Key, data);
    }


    public void SetString(string data)
    {
      Store.Set(Key, data);
    }

    public void SetBytes(byte[] data)
    {
      Store.Set(Key, data);
    }

    public bool GetBool()
    {
      return Store.Get<bool>(Key);
    }

    public ushort GetUInt16()
    {
      return Store.Get<ushort>(Key);
    }

    public int GetInt32()
    {
      return Store.Get<int>(Key);
    }

    public string GetString()
    {
      return Store.Get<string>(Key);
    }

    public byte[] GetBytes()
    {
      return Store.Get<byte[]>(Key);
    }
  }
}
