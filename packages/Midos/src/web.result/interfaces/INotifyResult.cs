namespace Microsoft.AspNetCore.Mvc
{
  public interface INotifyResult<T>
  {
    INotifyResult<T> StatusCode(int code);

    INotifyResult<T> Header(string key, string value);

    INotifyResult<T> Success(string message);

    INotifyResult<T> Danger(string message);
  }
}
