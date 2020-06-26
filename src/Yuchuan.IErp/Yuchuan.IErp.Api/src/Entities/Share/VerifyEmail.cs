using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yuchuan.IErp.Api
{
  [Table("verify_emails")]
  public class VerifyEmail
  {
    public int id { get; set; }

    public string email { get; set; }

    public string code { get; set; }

    public bool is_verified { get; set; }

    public DateTime expired_at { get; set; }
  }
}
