using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.DB
{
  [Table("items")]
  public class Item
  {
    [Key]
    public int id { get; set; }

    public int repository_id { get; set; }

    public string name { get; set; }

    public string model { get; set; }

    public string unit_of_measurement { get; set; }

    public double quantity { get; set; }

    public DateTime created_at { get; set; }

    public Item()
    {
      created_at = DateTime.Now;
    }
  }
}
