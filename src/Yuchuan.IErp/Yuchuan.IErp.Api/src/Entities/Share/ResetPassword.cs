using System.ComponentModel.DataAnnotations.Schema;

namespace Yuchuan.IErp.Api
{
  [Table("reset_passwords")]
  public class ResetPassword
  {
    public int id { get; set; }

    public int user_id { get; set; }

    public int verify_email_id { get; set; }
  }
}
