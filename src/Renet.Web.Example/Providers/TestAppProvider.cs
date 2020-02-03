using System;
using Microsoft.AspNetCore.Builder;

namespace Renet.Web.Example
{
  public class TestAppProvider : AppProvider
  {
    public override void Configure(IApplicationBuilder app)
    {
      Console.WriteLine("TestAppProvider called");
    }
  }
}
