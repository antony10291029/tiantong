using System;
using System.Collections.Generic;

namespace Midos.Account.Api
{
  public class Policy
  {
    public long Id { get; set; }

    public string Name { get; set; }

    public string Text { get; set; }

    public bool IsEnabeld { get; set; }

    public bool IsDeletable { get; set; }

    public DateTime CreatedAt { get; set; }

    public static Policy[] FromEntities(Policy[] entities)
    {
      foreach (var entity in entities) {
        entity.Id = 0;
        entity.IsEnabeld = true;
        entity.IsDeletable = true;
        entity.CreatedAt = DateTime.Now;
      }

      return entities;
    }
  }
}
