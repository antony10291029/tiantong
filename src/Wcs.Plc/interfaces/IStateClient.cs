namespace Wcs.Plc
{
  public interface IStateClient
  {
    void SetKey(string key);

    void SetLength(int length);

    void SetBool(bool data);

    void SetInt(int data);

    void SetString(string data);

    bool GetBool();

    int GetInt();

    string GetString();

  }
}
