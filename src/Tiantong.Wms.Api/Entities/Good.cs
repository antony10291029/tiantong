using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("goods")]
  public class Good : Entity
  {
    public int warehouse_id { get; set; }

    public List<int> category_ids { get; set; } = new List<int>();

    public string number { get; set; }

    public string name { get; set; }

    public string comment { get; set; } = "";

    public bool is_enabled { get; set; } = true;

    public DateTime created_at { get; set; } = DateTime.Now;

    public List<int> item_ids { get; set; } = new List<int>();

    public List<int> stock_ids { get; set; } = new List<int>();

    // [ForeignKey("good_id")]
    // public List<Item> items { get; set; }

    // [ForeignKey("good_id")]
    // public List<Stock> stocks { get; set; }
  }
}
