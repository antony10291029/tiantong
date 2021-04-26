namespace Microsoft.AspNetCore.Mvc
{
  [ValidateModel]
  public class BaseController: ControllerBase
  {
    public class OperationResult: MessageObject
    {
      public long Id { get; set; }
    }

    public object SuccessOperation(string msg, int id)
    {
      return NotifyResult.From(new OperationResult {
        Id = id
      }).Success(msg);
    }

    public object SuccessOperation(string msg)
    {
      return NotifyResult.FromVoid().Success(msg);
    }

    public object FailureOperation(string msg, int code = 400)
    {
      return NotifyResult.FromVoid().StatusCode(code).Danger(msg);
    }
  }
}
