using Midos.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Common.Entities
{
  public static class WmsTaskStatus
  {
    public const string Created = null;

    public const string Closed = "closed";

    public const string Started = "started";

    public const string Finished = "finished";
  }

  [Table("wms_task")]
  public class WmsTask: IEntity
  {
    [Key]
    [Column("ID")]
    public long Id { get; private set; }

    [Column("WORK_STATUS")]
    public string WorkStatus { get; private set; }

    public void Close()
    {
      WorkStatus = WmsTaskStatus.Closed;
    }

    public void Reset()
    {
      WorkStatus = WmsTaskStatus.Created;
    }

    public void Start()
    {
      WorkStatus = WmsTaskStatus.Started;
    }

    public void Finish()
    {
      WorkStatus = WmsTaskStatus.Finished;
    }
  }
}
