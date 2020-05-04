using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Iot.Entities
{
  [Table("http_watchers")]
  public class HttpWatcher
  {
    [Key]
    public virtual int id { get; set; }

    public virtual int state_id { get; set; }

    public virtual string url { get; set; }

    public virtual string value_key { get; set; }

    public virtual string data { get; set; }

    public virtual bool to_string { get; set; }
  }
}
