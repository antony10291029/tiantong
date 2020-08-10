using System.Collections.Generic;

namespace Namei.Wcs.Api
{
  public class CrashDoor
  {
    public const string First = "901";

    public const string Second = "902";

    public const string Third = "903";

    public static IEnumerable<string> Enumerate()
    {
      yield return First;
      yield return Second;
      yield return Third;
    }
  }
}
