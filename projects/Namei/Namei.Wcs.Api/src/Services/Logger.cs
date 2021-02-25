namespace Namei.Wcs.Api
{
  public class Logger
  {
    private DomainContext _domain;

    public Logger(DomainContext domain)
    {
      _domain = domain;
    }

    public void Log(string type, string key, string message)
    {
      _domain.Add(new Log {
        key = key,
        type = type,
        message = message
      });
      _domain.SaveChanges();
    }

    public void Info(string key, string message)
      => Log("info", key, message);

    public void Success(string key, string message)
      => Log("success", key, message);

    public void Warning(string key, string message)
      => Log("warning", key, message);

    public void Danger(string key, string message)
      => Log("danger", key, message);
  }
}
