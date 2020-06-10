using System.Collections.Generic;
using System.Linq;

namespace Tiantong.Iot.Entities
{
  public static class PlcStateType
  {
    public const string Bool = "bool";

    public const string UInt16 = "uint16";

    public const string Int32 = "int32";

    public const string String = "string";

    public static IEnumerable<string> Values()
    {
      yield return Bool;
      yield return Int32;
      yield return String;
      yield return UInt16;
    }

    public static bool IsValid(string model)
    {
      return !Values().Any(value => value == model);
    }
  }
}