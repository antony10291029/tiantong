namespace System
{
  public class KnownException: Exception, IKnownException
  {
    public int ErrorCode { get; private set; }

    public object[] ErrorData { get; private set; }

    public readonly static KnownException Unknown = new KnownException ("未知错误");

    public static KnownException Error(string message = null, int code = 400, object[] data = null)
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
