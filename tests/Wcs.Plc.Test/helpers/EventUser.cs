using System;

namespace Wcs.Plc.Test
{
  public class EventUser : IComparable
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public int CompareTo(object value)
    {
      return 0;
    }
  }
}
