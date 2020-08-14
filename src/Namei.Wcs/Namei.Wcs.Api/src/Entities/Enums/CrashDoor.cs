using System.Collections.Generic;

namespace Namei.Wcs.Api
{
  public class CrashDoor
  {
    public const string Floor1_1 = "151";

    public const string Floor2_1 = "251";

    public const string Floor3_1 = "351";

    public const string Floor4_1 = "451";

    public const string Floor1_2 = "152";

    public const string Floor2_2 = "252";

    public const string Floor3_2 = "352";

    public const string Floor4_2 = "452";

    public const string Floor1_3 = "153";

    public const string Floor2_3 = "253";

    public const string Floor3_3 = "353";

    public const string Floor4_3 = "453";

    public static string GetDoorIdFromLifter(string floor, string lifterId)
    {
      return $"{floor}5{lifterId}";
    }

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
