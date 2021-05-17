namespace Microsoft.AspNetCore.Mvc
{
  public class Result
  {
    public static IResult<T> From<T>(T data) => new Result<T>(data);

    public static IResult<Void> FromVoid() => new Result<Void>(null);

    public static IResult<object> FromObject(object data) => new Result<object>(data);
  }

  public static class NotifyResult
  {
    public static INotifyResult<T> From<T>(T data) where T: IMessageObject
      => new NotifyResult<T>(data);

    public static INotifyResult<IMessageObject> FromVoid()
      => new NotifyResult<IMessageObject>(new MessageObject());

    // @todo from object
  }
}
