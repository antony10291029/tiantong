using Microsoft.EntityFrameworkCore;

namespace Midos.Account.Api
{
  [Index(nameof(RoleId), nameof(PolicyId), IsUnique = true)]
  public class RolePolicy
  {
    public long Id { get; set; }

    public long RoleId { get; set; }

    public long PolicyId { get; set; }
  }
}
