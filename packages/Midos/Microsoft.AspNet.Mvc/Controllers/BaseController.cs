namespace Microsoft.AspNetCore.Mvc
{
  [ValidateModel]
  public class BaseController: ControllerBase
  {
    protected class OperationResult: MessageObject
    {
      public long Id { get; set; }
    }

    protected object SuccessOperation(string msg, int id)
    {
      return NotifyResult.From(new OperationResult {
        Id = id
      }).Success(msg);
    }

    protected object SuccessOperation(string msg)
    {
      return NotifyResult.FromVoid().Success(msg);
    }

    protected object FailureOperation(string msg, int code = 400)
    {
      return NotifyResult.FromVoid().StatusCode(code).Danger(msg);
    }
  }
}
