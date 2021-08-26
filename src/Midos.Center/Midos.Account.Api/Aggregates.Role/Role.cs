using Midos.Domain;
using System;

namespace Midos.Account.Api
{
  public class Role: IEntity
  {
    public long Id { get; set; }

    public string Name { get; set; }

    public bool IsEnabled { get; set; }

    public bool IsDeletable { get; set; }

    public DateTime CreatedAt { get; set; }

    public static Role FromEntity(Role role)
    {
      role.Id = 0;
      role.IsEnabled = true;
      role.IsDeletable = true;
      role.CreatedAt = DateTime.Now;

      return role;
    }
  }
}
