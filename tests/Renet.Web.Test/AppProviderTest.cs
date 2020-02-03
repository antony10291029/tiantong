using NUnit.Framework;
using System.Threading;
using Microsoft.AspNetCore.Builder;

namespace Renet.Web.Test
{
  [TestFixture]
  public class AppProviderTest
  {
    [Test]
    public void Test()
    {
      var server = new TestableServer();

      server.UseTokenSource(TestAppProvider.tokenSource)
        .Configure(app => app.UseProvider<TestAppProvider>())
        .Run();
    }
  }

  public class TestAppProvider : AppProvider
  {
    public static CancellationTokenSource tokenSource = new CancellationTokenSource();

    public override void Configure(IApplicationBuilder app)
    {
      tokenSource.Cancel();
    }
  }
}
