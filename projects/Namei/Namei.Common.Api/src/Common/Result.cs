namespace Namei.Common.Api
{
  public class Result
  {
    public string Message { get; set; }

    public string Status { get; set; } = "1";

    public string ErrorCode { get; set; } = "00";

    public void SetError(string message, string code)
    {
      Status = "0";
      Message = message;
      ErrorCode = code;
    }
  }

  public class DataResult<T>: Result
  {
    public T Data { get; set; }
  }
}
