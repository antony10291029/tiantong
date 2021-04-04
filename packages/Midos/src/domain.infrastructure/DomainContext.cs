using System;
using DBCore;
using DotNetCore.CAP;

namespace Midos.Domain
{
  public class DomainContext: DbContext
  {
    protected ICapPublisher Cap;

    public DomainContext(ICapPublisher cap)
    {
      Cap = cap;
    }

    public void Publish(string name, object data)
      => Cap.Publish(name, data);
  }
}
