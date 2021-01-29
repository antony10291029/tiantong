using System;
using System.Reflection;
using System.Dynamic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Renet.Web
{
  public class RouterBuilder
  {
    private string _pattern;

    private string[] _controller;

    private HttpMethodRouteConstraint _methods;

    public RouterBuilder Pattern(string pattern)
    {
      _pattern = pattern;

      return this;
    }

    public RouterBuilder Controller(string controller)
    {
      _controller = controller.Split(".");

      var assembly = Assembly.GetCallingAssembly().GetName().Name;
      var typeName = $"{assembly}.{_controller[0]}Controller";
      var type = Type.GetType(typeName);

      if (type != null) {
        if (type.GetMethod(_controller[1]) == null) {
          throw new ControllerNotFoundException(controller);
        }
      }

      return this;
    }

    public RouterBuilder Methods(params string[] methods)
    {
      _methods = new HttpMethodRouteConstraint(methods);

      return this;
    }

    public void Build(IEndpointRouteBuilder endpoints)
    {
      dynamic defaults = new ExpandoObject();
      dynamic constraints = new ExpandoObject();

      if (_methods != null) {
        constraints.methods = _methods;
      }

      defaults.controller = _controller[0];
      defaults.action = _controller[1];

      endpoints.MapControllerRoute(
        _controller[0] + _controller[1],
        _pattern,
        (object) defaults,
        (object) constraints
      );
    }
  }
}
