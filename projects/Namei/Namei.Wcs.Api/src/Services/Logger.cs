namespace Namei.Wcs.Api
{
  public class Logger
  {
    private DomainContext _domain;

    public Logger(DomainContext domain)
    {
      _domain = domain;
    }

    public void Save(Log log)
    {
      _domain.Add(log);
      _domain.SaveChanges();
    }
  }
}
