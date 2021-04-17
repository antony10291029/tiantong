using System.Reflection.Emit;
using System.Collections.Generic;

namespace Namei.Wcs.Api
{
  public class AutomatedDoor
  {
    public const string Floor1_1 = "101";

    public const string Floor1_2 = "102";

    public const string Floor1_3 = "103";

    public const string Floor1_4 = "104";

    public const string Floor1_5 = "105";

    public const string Floor1_6 = "106";

    public const string Floor1_7 = "107";

    public const string Floor2_1 = "201";

    public const string Floor2_2 = "202";

    public static IEnumerable<string> Enumerate()
    {
      yield return Floor1_1;
      yield return Floor1_2;
      yield return Floor1_3;
      yield return Floor1_4;
      yield return Floor1_5;
      yield return Floor1_6;
      yield return Floor1_7;
      yield return Floor2_1;
      yield return Floor2_2;
    }
  }
}
