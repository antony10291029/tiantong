using System;

namespace Midos.Services.Logging
{
  public class LogData
  {
    public string Level { get; set; }

    public string Category { get; set; }

    public string EventId { get; set; }

    public object Exception { get; set; }

    public object Data { get; set; }

    public DateTime CreatedAt { get; set; }
  }
}
