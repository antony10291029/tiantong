namespace Tiantong.Iot
{
  public interface IStateDriver
  {
    void UseBool();

    void UseUInt16();

    void UseInt32();

    void UseString(int length);

    void UseBytes(int length);

    void UseAddress(string key);

    //

    void SetBool(bool data);

    void SetUInt16(ushort data);

    void SetInt32(int data);

    void SetString(string data);

    void SetBytes(byte[] data);

    //

    bool GetBool();

    ushort GetUInt16();

    // todo remove
    int GetInt32();

    string GetString();

    byte[] GetBytes();

  }

}
