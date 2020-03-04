using System;

namespace Tiantong.Wms.Api
{
  public interface IAuth
  {
    User User { get; }

    bool NeedToRefresh { get; }

    void Ensure();

    void EnsureType(string type);

    void EnsureOwner();

    (string, DateTime, DateTime) Encode(User user);
  }
}
