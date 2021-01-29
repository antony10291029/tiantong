using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

namespace Renet.Web
{
  public abstract class RouterProvider : AppProvider
  {
    private List<RouterBuilder> _builders = new List<RouterBuilder>();

    public override void Configure(IApplicationBuilder app)
    {
      Define();
      app.UseEndpoints(endpoints => {
        foreach (var builder in _builders) {
          builder.Build(endpoints);
        }
      });
    }

    public abstract void Define();

    protected RouterBuilder Get(string pattern, string controller)
    {
      var builder = new RouterBuilder();

      builder.Methods("get")
        .Pattern(pattern)
        .Controller(controller);
      _builders.Add(builder);

      return builder;
    }

    protected RouterBuilder Post(string pattern, string controller)
    {
      var builder = new RouterBuilder();

      builder.Methods("post")
        .Pattern(pattern)
        .Controller(controller);
      _builders.Add(builder);

      return builder;
    }

    protected RouterBuilder Put(string pattern, string controller)
    {
      var builder = new RouterBuilder();
      
      builder.Methods("put")
        .Pattern(pattern)
        .Controller(controller);
      _builders.Add(builder);

      return builder;
    }

    protected RouterBuilder Patch(string pattern, string controller)
    {
      var builder = new RouterBuilder();
      
      builder.Methods("patch")
        .Pattern(pattern)
        .Controller(controller);
      _builders.Add(builder);

      return builder;
    }

    protected RouterBuilder Delete(string pattern, string controller)
    {
      var builder = new RouterBuilder();

      builder.Methods("delete")
        .Pattern(pattern)
        .Controller(controller);
      _builders.Add(builder);

      return builder;
    }

    protected RouterBuilder Head(string pattern, string controller)
    {
      var builder = new RouterBuilder();
      
      builder.Methods("head")
        .Pattern(pattern)
        .Controller(controller);
      _builders.Add(builder);

      return builder;
    }

    protected RouterBuilder Options(string pattern, string controller)
    {
      var builder = new RouterBuilder();
      
      builder.Methods("options")
        .Pattern(pattern)
        .Controller(controller);
      _builders.Add(builder);

      return builder;
    }

    protected RouterBuilder Any(string pattern, string controller)
    {
      var builder = new RouterBuilder();
      
      builder.Methods()
        .Pattern(pattern)
        .Controller(controller);
      _builders.Add(builder);

      return builder;
    }
  }
}
