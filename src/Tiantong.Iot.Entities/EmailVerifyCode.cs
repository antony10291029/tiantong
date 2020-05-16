using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Iot
{
  [Table("email_verify_code")]
  public class EmailVerifyCode
  {
    [Key]
    public virtual int id { get; set; }

    public virtual string email { get; set; }

    public virtual string verify_code { get; set; }

    public virtual bool is_verified { get; set; } = false;

    public virtual DateTime expired_at { get; set; } = DateTime.Now.AddMinutes(30);

    public virtual DateTime created_at { get; set; } = DateTime.Now;
  }
}
