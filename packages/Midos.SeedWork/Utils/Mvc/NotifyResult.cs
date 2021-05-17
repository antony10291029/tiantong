namespace Microsoft.AspNetCore.Mvc
{
  public class NotifyResult<T>: Result<T>, INotifyResult<T> where T: IMessageObject
  {
    private readonly T _data;

    public NotifyResult(T data): base(data)
    {
      _data = data;
    }

    public new INotifyResult<T> StatusCode(int code)
    {
      base.StatusCode(code);

      return this;
    }

    public new INotifyResult<T> Header(string key, string value)
    {
      base.Header(key, value);

      return this;
    }

    private void SetType(string type)
    {
      Header("Midos-Notify-Type", type);
    }

    private void SetMessage(string message)
    {
      _data.Message = message;
    }

    public INotifyResult<T> Success(string message)
    {
      SetType("Success");
      SetMessage(message);
      StatusCode(201);

      return this;
    }

    public INotifyResult<T> Danger(string message)
    {
      SetType("Danger");
      SetMessage(message);
      StatusCode(400);

      return this;
    }
  }
}
