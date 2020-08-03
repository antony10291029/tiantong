namespace Namei.Wcs.Api
{
  public class Logger
  {
    private DomainContext _domain;

    public Logger(DomainContext domain)
    {
      _domain = domain;
    }

    public void Log(string key, string type, string message)
    {
      _domain.Add(new Log {
        key = key,
        type = type,
        message = message
      });
      _domain.SaveChanges();
    }
  }
}
