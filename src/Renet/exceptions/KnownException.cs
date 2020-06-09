using System;

namespace Renet
{
  public interface IKnownException
  {
    string Message { get; }

    int ErrorCode { get; }

    object[] ErrorData { get; }
  }

  public class KnownException: Exception, IKnownException
  {
    public int ErrorCode { get; private set; }

    public object[] ErrorData { get; private set; }

    public readonly static KnownException Unknown = new KnownException ("未知错误");

    public static KnownException Error(string message = "", int code = 0, object[] data = null)
    {
      return new KnownException (message) {
        ErrorCode = code,
        ErrorData = data,
      };
    }

    private KnownException(string msg): base(msg)
    {

    }
  }
}