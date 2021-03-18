using Midos.Center.Entities;

namespace Midos.Center.Events
{
  public class TaskOrderCreate
  {
    public const string Message = "tasks.create";

    public string Key { get; init; }

    public TaskData Data { get; init; }

    private TaskOrderCreate() {}

    public static TaskOrderCreate From(string key, TaskData data)
    {
      return new TaskOrderCreate {
        Key = key,
        Data = data,
      };
    }
  }
}
