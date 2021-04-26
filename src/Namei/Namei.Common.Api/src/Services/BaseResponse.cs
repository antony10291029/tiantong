namespace Namei.Common.Api
{
  public class Response<T>
  {
    public string Message { get; set; }

    public string Status { get; set; }

    public string ErrorCode { get; set; }

    public T Data { get; set; }
  }

  public class Data
  {
    public string id { get; set; }
  }

  public class MyResponse: Response<Data>
  {
    public MyResponse()
    {
      var data = new Response<Data>();

      data.Data.id = "100";
    }
  }
}
