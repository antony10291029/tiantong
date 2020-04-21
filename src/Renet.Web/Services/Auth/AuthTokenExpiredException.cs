using System;

namespace Renet.Web
{
  public class AuthTokenExpiredException: Exception
  {
    public AuthTokenExpiredException(string message): base(message)
    {

    }
  }
}
