using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yuchuan.IErp.Api
{
  [Table("project_users")]
  public class ProjectUser
  {
    [Key]
    public int id { get; set; }

    public int project_id { get; set; }

    public int user_id { get; set; }

    public string role { get; set; }
  }
}
