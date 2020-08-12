using System.Linq;
using System.Collections.Generic;
namespace Tiantong.Iot.Entities
{
  public static class PlcModel
  {
    public const string MC3EBinary = "mc3e-binary";

    public const string MC1EBinary = "mc1e-binary";

    public const string S7200Smart = "s7200smart";

    public const string Test = "test";

    public static IEnumerable<string> Values()
    {
      yield return MC3EBinary;
      yield return MC1EBinary;
      yield return S7200Smart;
      yield return Test;
    }

    public static bool IsValid(string model)
    {
      return !Values().Any(value => value == model);
    }
  }
}
