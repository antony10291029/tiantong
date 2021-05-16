using DotNetCore.CAP;

namespace Midos.SeedWork
{
  public class UnitOfWork: IUnitOfWork
  {
    private readonly EFContext _context;

    private readonly ICapPublisher _cap;

    public UnitOfWork(EFContext context, ICapPublisher cap)
    {
      _cap = cap;
      _context = context;
    }

    public void Publish<TDomainEvent>(string key, TDomainEvent data)
      where TDomainEvent: DomainEvent
      => _cap.Publish(key, data);

    public void BeginTransaction()
      => _context.Database.BeginTransaction();

    public void Rollback()
      => _context.Database.RollbackTransaction();

    public void Commit()
      => _context.Database.CommitTransaction();

    public void SaveChanges()
      => _context.SaveChanges();
  }
}
