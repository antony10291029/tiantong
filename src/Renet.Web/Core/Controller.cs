using Microsoft.AspNetCore.Mvc;

namespace Renet.Web
{
  [ValidateModel]
  public class BaseController : ControllerBase
  {
    public object JsonMessage(string message)
    {
      return new { message };
    }

    public object SuccessOperation(string message)
    {
      throw new SuccessOperation(message);
    }

    public object SuccessOperation(object body)
    {
      throw new SuccessOperation(body);
    }

    public object SuccessOperation(string message, object id)
    {
      throw new SuccessOperation(new { message, id });
    }

    public object FailureOperation(string message)
    {
      throw new FailureOperation(message);
    }

    public object FailureOperation(object body)
    {
      throw new FailureOperation(body);
    }
  }
}
