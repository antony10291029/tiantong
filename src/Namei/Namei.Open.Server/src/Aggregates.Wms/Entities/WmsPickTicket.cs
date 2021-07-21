using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Open.Server
{
  [Table("wms_pick_ticket")]
  public class WmsPickTicket
  {
    [Column("ID")]
    public long Id { get; set; }

    [Column("CODE")]
    public string Code { get; set; }

    [Column("SHIP_TO_NAME")]
    public string ShipToName { get; set; }
  }
}
