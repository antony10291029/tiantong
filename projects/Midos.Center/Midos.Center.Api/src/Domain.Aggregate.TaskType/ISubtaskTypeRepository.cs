using Midos.Domain;

namespace Midos.Center.Aggregates
{
  public interface ISubtaskTypeRepository: IRepository<SubtaskType>
  {

  }

  public class SubtaskTypeRepository: Repository<SubtaskType>, ISubtaskTypeRepository
  {
    public SubtaskTypeRepository(DomainContext domain): base(domain)
    {

    }
  }
}
