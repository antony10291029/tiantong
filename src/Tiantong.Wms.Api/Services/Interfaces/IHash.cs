namespace Tiantong.Wms.Api
{
  public interface IHash
  {
    string Make(string content);

    bool Match(string content, string hashedContent);
  }
}
