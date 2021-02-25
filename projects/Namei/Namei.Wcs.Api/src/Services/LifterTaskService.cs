using System;
using System.Collections.Generic;

namespace Namei.Wcs.Api
{
  public class LifterTaskService
  {
    private Dictionary<string, DateTime> _tasks = new Dictionary<string, DateTime>();

    public void Set(string barcode)
    {
      _tasks[barcode] = DateTime.Now;
    }

    public DateTime Get(string barcode)
    {
      if (_tasks.ContainsKey(barcode)) {
        return _tasks[barcode];
      } else {
        return DateTime.MinValue;
      }
    }
  }
}
