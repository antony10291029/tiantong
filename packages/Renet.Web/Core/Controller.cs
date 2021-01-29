using Microsoft.AspNetCore.Mvc;

namespace Renet.Web
{
  [ValidateModel]
  public class BaseController : ControllerBase
  {
    protected object JsonMessage(string message)
    {
      return new { message };
    }

    protected object SuccessOperation(string message)
    {
      throw new SuccessOperation(message);
    }

    protected object SuccessOperation(object body)
    {
      throw new SuccessOperation(body);
    }

    protected object SuccessOperation(string message, object id)
    {
      throw new SuccessOperation(new { message, id });
    }

    protected object FailureOperation(string message)
    {
      throw new FailureOperation(message);
    }

    protected object FailureOperation(object body)
    {
      throw new FailureOperation(body);
    }
  }
}
