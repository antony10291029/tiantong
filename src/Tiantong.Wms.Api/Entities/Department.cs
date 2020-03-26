using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("departments")]
  public class Department : Entity
  {
    [Key]
    public int id { get; set; }

    public int warehouse_id { get; set; }

    public string name { get; set; }

  }
}
