namespace Microsoft.AspNetCore.Mvc
{
  public interface INotifyResult<T>: IResult<T>
  {
    new INotifyResult<T> StatusCode(int code);

    new INotifyResult<T> Header(string key, string value);

    INotifyResult<T> Success(string message);

    INotifyResult<T> Danger(string message);
  }
}
