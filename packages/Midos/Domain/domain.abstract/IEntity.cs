namespace Midos.Domain
{
  public interface IEntity<TKey>
  {
    TKey Id { get; }
  }

  public interface IEntity: IEntity<long>
  {

  }
}
