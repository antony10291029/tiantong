namespace Wcs.Plc
{
  public interface IStateDriver
  {
    void SetAddress(string key, int length);

    void SetBool(bool data);

    // todo remove
    void SetInt(int data);

    void SetUShort(ushort data)
    {
      SetInt(data);
    }

    void SetString(string data);

    bool GetBool();


    // todo remove
    int GetInt();

    ushort GetUShort()
    {
      return (ushort) GetInt();
    }

    string GetString();

  }
}
