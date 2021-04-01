using Midos.Center.Entities;

namespace Midos.Center.Events
{
  public struct TaskOrderCreate
  {
    public const string Message = "tasks.create";

    public string Key { get; init; }

    public TaskData Data { get; init; }

    public static TaskOrderCreate From(string key, TaskData data)
    {
      return new TaskOrderCreate {
        Key = key,
        Data = data,
      };
    }
  }
}
