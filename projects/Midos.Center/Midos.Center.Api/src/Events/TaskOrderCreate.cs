using Midos.Domain;

namespace Midos.Center.Events
{
  public struct TaskOrderCreate
  {
    public const string Message = "tasks.create";

    public string Key { get; init; }

    public Record Data { get; init; }

    public string Code { get; init; }

    public static TaskOrderCreate From(
      string key,
      Record data,
      string code = null
    ) {
      return new TaskOrderCreate {
        Key = key,
        Code = code,
        Data = data,
      };
    }
  }
}
