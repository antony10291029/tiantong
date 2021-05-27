using Midos.Domain;
using Newtonsoft.Json;

namespace Namei.Wcs.Aggregates
{
  public record AgcTaskCreate: DomainEvent
  {
    public const string @event = "agc.tasks.create";

    public string Type { get; init; }

    public long TypeId { get; set; }

    public string Position { get; init; }

    public string Destination { get; init; }

    public string PodCode { get; set; }

    public string TaskId { get; set; }

    public string Priority { get; set; }

    public string PalletCode
    {
      get => PodCode;
      set => PodCode = value;
    }

    public static AgcTaskCreate From(
      string position,
      string destination,
      long typeId = 0,
      string type = "",
      string podCode = "",
      string taskId = "",
      string priority = ""
    ) => new() {
      TypeId = typeId,
      Type = type,
      Position = position,
      Destination = destination,
      Priority = priority,
      PodCode = podCode,
      TaskId = taskId,
    };
  }
}
