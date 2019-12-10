using System.Collections.Generic;

namespace Wcs.Plc
{
  public interface IStateManager
  {
    string Name { get; set; }

    Dictionary<string, IState> States { get; }

    IStateBit Bit(string bit);

    IStateBits Bits(string bits, int length = 1);

    IStateWord Word(string key);

    IStateWords Words(string key, int length = 1);
  }
}