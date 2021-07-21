using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Open.Server
{
  [Table("OITM")]
  public class SapItem
  {
    [Key]
    public string ItemCode { get; set; }

    public string ItemName { get; set; }

    public string BuyUnitMsr { get; set; }
  }
}
