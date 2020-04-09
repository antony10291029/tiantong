using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Tiantong.Wms.Api
{
  [Table("users")]
  public class User : Entity
  {
    [Required]
    [EmailAddress]
    public virtual string email { get; set; }

    [JsonIgnore]
    public virtual string password { get; set; }

    public virtual string name { get; set; } = "";

    public virtual string type { get; set; }

    public virtual bool is_enabled { get; set; } = true;

    public virtual bool is_deleted { get; set; } = false;

    public virtual DateTime created_at { get; set; } = DateTime.Now;

  }
}
