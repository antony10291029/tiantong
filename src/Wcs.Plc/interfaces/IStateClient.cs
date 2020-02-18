namespace Wcs.Plc
{
  public interface IStateClient
  {
    void SetAddress(string key, int length);

    void SetBool(bool data);

    void SetInt(int data);

    void SetString(string data);

    bool GetBool();

    int GetInt();

    string GetString();

  }
}
