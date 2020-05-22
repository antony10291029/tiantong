wusing System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Iot.Winforms
{
  [Table("key_values")]
  public class KeyValue
  {
    [Key]
    public virtual string key { get; set; }

    public virtual string value { get; set; }
  }
}
