using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.DB
{
  [Table("item_outgoing_records")]
  public class ItemOutgoingRecord
  {
    [Key]
    public int id { get; set; }

    public int item_id { get; set; }

    public int operator_id { get; set; }

    public double quantity { get; set; }

    public string method { get; set; }

    public DateTime created_at { get; set; }

    public ItemOutgoingRecord()
    {
      created_at = DateTime.Now;
    }
  }
}
