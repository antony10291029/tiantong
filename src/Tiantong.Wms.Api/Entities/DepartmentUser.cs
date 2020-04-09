using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Wms.Api
{
  [Table("department_users")]
  public class DepartmentUser : Entity
  {
    public int warehouse_id { get; set; }

    public int department_id { get; set; }

    public int user_id { get; set; }

    public string role { get; set; } = KeeperRoles.Admin;

    [ForeignKey("user_id")]
    public User user { get; set; }

  }
}
