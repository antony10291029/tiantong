using System;

namespace Renet.Web
{
  public class ControllerNotFoundException : Exception
  {
    private string _controller;

    public override string Message
    {
      get => $"controller or method `{_controller}` is not found";
    }

    public override string StackTrace
    {
      get => null;
    }

    public ControllerNotFoundException(string controller)
    {
      _controller = controller;
    }
  }
}
