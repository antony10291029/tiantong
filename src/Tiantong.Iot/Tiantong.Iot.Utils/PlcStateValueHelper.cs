using System.Collections;

namespace Tiantong.Iot.Utils
{
  public class MelsecStateHelper
  {
    public static bool GetBit(ushort value, int position)
      => new BitArray(new int[] { value })[position];

    public static bool GetBit(string value, int position)
      => GetBit(ushort.Parse(value), position);
  }
}
