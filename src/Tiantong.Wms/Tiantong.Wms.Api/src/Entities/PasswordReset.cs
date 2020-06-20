using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("password_resets")]
  public class PasswordReset: Entity
  {
    public virtual int user_id { get; set; }

    public virtual int email_verification_id { get; set; }

    [ForeignKey("email_verification_id")]
    public EmailVerification email_verification { get; set; }
  }
}
