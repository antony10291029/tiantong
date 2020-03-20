using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("items")]
  public class Item : Entity
  {
    [Key]
    public int id { get; set; }

    public int warehouse_id { get; set; }

    public string number { get; set; }

    public int[] category_ids { get; set; } = new int[] {};

    public string name { get; set; }

    public string specification { get; set; }

    public double reference_purchase_price { get; set; } = 0;

    public string comment { get; set; } = "";

    public bool is_enabled { get; set; } = true;

    public DateTime created_at { get; set; } = DateTime.Now;

    [ForeignKey("item_id")]
    public List<Stock> stocks { get; set; }

  }
}
