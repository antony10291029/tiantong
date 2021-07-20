using Midos.Domain;
using System;

namespace Midos.Center.Aggregates
{
  public class User: IEntity
  {
    public long Id { get; set; }

    public long RoleId { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public DateTime CreatedAt { get; set; }
  }
}
