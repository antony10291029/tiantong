using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Open.Server
{
  [Table("MesWoOrder")]
  public class MesWoOrder
  {
    [Key]
    public string WoOrderNo { get; set; }

    public string FormulaNo { get; set; }
  }
}
