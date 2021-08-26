using Microsoft.EntityFrameworkCore;
using Midos.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace Midos.Account.Api
{
  [Index(nameof(Email), IsUnique = true)]
  [Index(nameof(Mobile), IsUnique = true)]
  [Index(nameof(Username), IsUnique = true)]
  public class User: IEntity
  {
    public long Id { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public string Mobile { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string Name { get ; set; }

    public bool IsEnabled { get; set; }

    public bool IsDeletable { get; set; }

    public DateTime CreatedAt { get; set; }

    public static User FromEntity(User user)
    {
      user.Id = 0;
      user.IsEnabled = true;
      user.IsDeletable = true;
      user.CreatedAt = DateTime.Now;

      return user;
    }
  }
}
