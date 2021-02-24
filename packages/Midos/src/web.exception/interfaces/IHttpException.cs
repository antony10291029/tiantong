namespace Microsoft.AspNetCore.Http
{
  public interface IHttpException
  {
    int Status { get; }

    string Body { get; }
  }
}
