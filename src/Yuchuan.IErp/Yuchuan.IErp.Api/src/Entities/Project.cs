using Newtonsoft.Json;
using Renet.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yuchuan.IErp.Api
{
  [Table("projects")]
  public class Project
  {
    [Key]
    public int id { get; set; }

    public string type { get ; set; }

    [StringRange(2, 20, ErrorMessage = "名称长度必须在 2～20 之间")]
    public string name { get; set; } = "";

    [StringLength(20, ErrorMessage = "编号长度必须在 2～20 之间")]
    public string number { get; set; } = "";

    [StringLength(200, ErrorMessage = "备注长度必须小于 200")]
    public string comment { get; set; } = "";

    public string region { get; set; } = "";

    public string province { get; set; } = "";

    public string city { get; set; } = "";

    public bool is_enabled { get; set; } = true;

    public DateTime created_at { get; set; } = DateTime.Now;

    [JsonIgnore]
    [ForeignKey("project_id")]
    public List<ProjectUser> Users { get; set; }

    [JsonIgnore]
    [ForeignKey("project_id")]
    public List<ProjectDevice> Devices { get; set; }
  }
}
