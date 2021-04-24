namespace Midos.Domain
{
  public interface IRepositoryFactory
  {
    IRepository<TEntity, TKey> Resolve<TEntity, TKey>()
      where TEntity: class, IEntity<TKey>;

    IRepository<TEntity> Resolve<TEntity>()
      where TEntity: class, IEntity;
  }

  public class RepositoryFactory: IRepositoryFactory
  {
    private DomainContext _domain;

    public RepositoryFactory(DomainContext domain)
    {
      _domain = domain;
    }

    public IRepository<TEntity, TKey> Resolve<TEntity, TKey>()
      where TEntity: class, IEntity<TKey>
    {
      return new Repository<TEntity, TKey>(_domain);
    }

    public IRepository<TEntity> Resolve<TEntity>()
      where TEntity: class, IEntity
    {
      return new Repository<TEntity>(_domain);
    }
  }
}
