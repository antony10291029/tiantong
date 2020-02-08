using System.Threading.Tasks;

namespace Wcs.Plc
{
  public interface IStateClient
  {
    IStateClient SetKey(string key);

    IStateClient SetLength(int length);

    Task SetWord(int data);

    Task SetWords(string data);

    Task SetBit(bool data);

    Task SetBits(string data);

    Task<int> GetWord();

    Task<string> GetWords();

    Task<bool> GetBit();

    Task<string> GetBits();
  }
}