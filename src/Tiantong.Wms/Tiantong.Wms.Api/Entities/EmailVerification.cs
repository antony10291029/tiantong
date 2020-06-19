using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("email_verifications")]
  public class EmailVerification: Entity
  {
    [EmailAddress(ErrorMessage = "邮箱地址无效")]
    public virtual string email { get; set; }

    public virtual string code { get; set; }

    public virtual bool is_verified { get; set; } = false;

    public DateTime expired_at { get; set; }
    
  }
}
