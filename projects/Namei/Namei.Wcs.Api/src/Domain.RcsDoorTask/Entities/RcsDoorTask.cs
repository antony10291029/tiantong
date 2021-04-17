using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Midos.Domain;

namespace Namei.Wcs.Api
{
  public static class RcsDoorTaskStatus
  {
    public const string Requested = "requested";

    public const string Entered = "entered";

    public const string Left = "left";
  }

  public class RcsDoorTask: IEntity<string>
  {
    [Key]
    [Column("Uuid")]
    public string Id { get; private set; }

    public string DoorId { get; private set; }

    public string Status { get; private set; }

    public int RetryCount { get; private set; }

    public DateTime RequestedAt { get; private set; }

    public DateTime EnteredAt { get; private set; }

    public DateTime LeftAt { get; private set; }

    private RcsDoorTask() {}

    public void Request(string doorId)
    {
      RetryCount = 0;
      DoorId = doorId;
      Status = RcsDoorTaskStatus.Requested;
      RequestedAt = DateTime.Now;
      EnteredAt = DateTime.MinValue;
      LeftAt = DateTime.MinValue;
    }

    public void Enter()
    {
      Status = RcsDoorTaskStatus.Entered;
      EnteredAt = DateTime.Now;
    }

    public void Leave()
    {
      Status = RcsDoorTaskStatus.Left;
      LeftAt = DateTime.Now;
    }

    public void Retry()
    {
      RetryCount++;
      Status = RcsDoorTaskStatus.Requested;
      EnteredAt = DateTime.MinValue;
      LeftAt = DateTime.MinValue;
    }

    public static RcsDoorTask From(string uuid, string doorId)
      => new RcsDoorTask {
        Id = uuid,
        DoorId = doorId,
        Status = RcsDoorTaskStatus.Requested,
        RequestedAt = DateTime.Now,
        EnteredAt = DateTime.MinValue,
        LeftAt = DateTime.MinValue,
        RetryCount = 0,
      };

    public static RcsDoorTask From(RcsDoorEvent param)
      => From(
        uuid: param.Uuid,
        doorId: param.DoorId
      );
  }
}
