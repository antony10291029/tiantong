using Midos.Center.Entities;

namespace Midos.Center.Events
{
  public class TaskOrderCreate
  {
    public const string Message = "midos.tasks.create";

    public string Key { get; set; }

    public string Data { get; set; }

    private TaskOrderCreate() {}

    public static TaskOrderCreate From(string key, string data)
    {
      return new TaskOrderCreate {
        Key = key,
        Data = data,
      };
    }
  }
}
