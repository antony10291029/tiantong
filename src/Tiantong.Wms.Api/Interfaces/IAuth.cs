using System;
using Tiantong.Wms.DB;
using Microsoft.AspNetCore.Http;

namespace Tiantong.Wms.Api
{
  public interface IAuth
  {
    User User { get; }

    void Ensure();

    void EnsureRoles(params string[] roles);

    (string, DateTime, DateTime) Encode(User user);
  }
}
