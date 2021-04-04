using System;

namespace Midos.Domain
{
  public interface IUnitOfWork
  {
    int SaveChanges();

    void SaveChanges(Action handler, Action<Exception> error = null);

    void Publish(string name, object data);
  }
}
