using Microsoft.EntityFrameworkCore;

namespace Midos.Account.Api
{
  [Index(nameof(UserId), nameof(RoleId), IsUnique = true)]
  public class UserRole
  {
    public long Id { get; set; }

    public long UserId { get; set; }

    public long RoleId { get; set; }
  }
}
