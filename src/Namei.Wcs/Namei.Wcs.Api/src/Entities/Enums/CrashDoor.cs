using System.Collections.Generic;

namespace Namei.Wcs.Api
{
  public class CrashDoor
  {
    public const string Floor1_1 = "911";

    public const string Floor2_1 = "921";

    public const string Floor3_1 = "931";

    public const string Floor4_1 = "941";

    public const string Floor1_2 = "912";

    public const string Floor2_2 = "922";

    public const string Floor3_2 = "932";

    public const string Floor4_2 = "942";

    public const string Floor1_3 = "913";

    public const string Floor2_3 = "923";

    public const string Floor3_3 = "933";

    public const string Floor4_3 = "943";

    public static IEnumerable<string> Enumerate()
    {
      yield return Floor1_1;
      yield return Floor2_1;
      yield return Floor3_1;
      yield return Floor4_1;
      yield return Floor1_2;
      yield return Floor2_2;
      yield return Floor3_2;
      yield return Floor4_2;
      yield return Floor1_3;
      yield return Floor2_3;
      yield return Floor3_3;
      yield return Floor4_3;
    }
  }
}
