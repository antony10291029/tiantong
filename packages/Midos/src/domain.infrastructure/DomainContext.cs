using System;
using DBCore;
using DotNetCore.CAP;

namespace Midos.Domain
{
  public class DomainContext: DbContext, IUnitOfWork
  {
    protected ICapPublisher Cap;

    public DomainContext(ICapPublisher cap)
    {
      Cap = cap;
    }

    public void SaveChanges(Action handler, Action<Exception> error)
      => SaveChanges(handler, error);

    public void Publish(string name, object data)
      => Cap.Publish(name, data);
  }
}
