using System.Collections.Generic;

namespace Namei.Wcs.Aggregates
{
  public class RcsAgcTaskStatus
  {
    public static string Created = "created";

    public static string Started = "started";

    public static string Finished = "finished";

    public static string Closed = "closed";

    public static IEnumerable<string> Enumerate()
    {
      yield return Created;
      yield return Started;
      yield return Finished;
      yield return Closed;
    }
  }
}
