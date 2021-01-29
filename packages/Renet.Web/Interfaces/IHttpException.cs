namespace Renet.Web
{
  public interface IHttpException
  {
    int Status { get; }

    string Body { get; }
  }
}
