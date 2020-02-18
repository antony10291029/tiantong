using System.Threading.Tasks;

namespace Wcs.Plc
{
  public interface IStateClient
  {
    IStateClient SetKey(string key);

    IStateClient SetLength(int length);

    Task SetInt(int data);

    Task SetString(string data);

    Task SetBool(bool data);

    Task<int> GetInt();

    Task<string> GetString();

    Task<bool> GetBool();

  }
}
