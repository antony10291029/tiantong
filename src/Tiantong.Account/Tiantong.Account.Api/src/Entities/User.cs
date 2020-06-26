using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Account.Api
{
  [Table("users")]
  public class User
  {
    [Key]
    public int id { get; set; }

    public string type { get; set; }

    public string name { get; set; }

    public string email { get; set; }

    public string password { get; set; }

    public string avatar_url { get; set; }

    public bool is_enabled { get; set; }

    public DateTime created_at { get; set; }
  }
}
