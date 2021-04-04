using Midos.Domain;
using System.Linq;

namespace Midos.Center.Aggregates
{
  public interface ITaskTypeRepository: IRepository<TaskType>
  {
    TaskType FindByKey(string key);
  }

  public class TaskTypeRepository: Repository<TaskType>, ITaskTypeRepository
  {
    public TaskTypeRepository(DomainContext domain): base(domain) {}

    public TaskType FindByKey(string key)
      => DomainContext.Set<TaskType>().FirstOrDefault();
  }
}
