using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Account.Api
{
  [Table("email_verifications")]
  public class EmailVerification
  {
    [Key]
    public int id { get; set; }

    public string address { get; set; }

    public string key { get; set; }

    public string code { get; set; }

    public int error_count { get; set; } = 0;

    public DateTime expired_at { get; set; }
  }
}
