using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Midos.SeedWork.Services.Logging
{
  [Index(nameof(App))]
  [Index(nameof(Category))]
  [Index(nameof(CreatedAt))]
  [Index(nameof(EventId))]
  [Index(nameof(Level))]
  [Index(nameof(Server))]
  public class LogData
  {
    public long Id { get; set; }

    public string App { get; set; }

    public string Server { get; set; }

    public string Level { get; set; }

    public string Category { get; set; }

    public string EventId { get; set; }

    [Column("Data")]
    public string IData { get; private set; }

    [Column("Exception")]
    public string IException { get; private set; }

    public DateTime CreatedAt { get; set; }

    [NotMapped]
    public object Data
    {
      get => JsonSerializer.Deserialize<object>(IData);
      set => IData = JsonSerializer.Serialize(value);
    }

    [NotMapped]
    public object Exception
    {
      get => JsonSerializer.Deserialize<object>(IException);
      set => IException = JsonSerializer.Serialize(value);
    }
  }
}
