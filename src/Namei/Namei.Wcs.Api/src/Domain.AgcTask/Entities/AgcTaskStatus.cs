using System.Collections.Generic;

namespace Namei.Wcs.Aggregates
{
  public class AgcTaskStatus
  {
    public const string Created = "created";

    public const string Finished = "finished";

    public const string Closed = "closed";

    public static IEnumerable<string> Enumerate()
    {
      yield return Created;
      yield return Finished;
      yield return Closed;
    }
  }
}
