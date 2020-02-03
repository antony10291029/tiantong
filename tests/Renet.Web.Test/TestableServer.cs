using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Renet.Web.Test
{
  public class TestableServer
  {
    public CancellationTokenSource TokenSource { get; set; }
      = new CancellationTokenSource();

    private Action<IApplicationBuilder> _configurer;

    public TestableServer Configure(Action<IApplicationBuilder> action)
    {
      _configurer = action;

      return this;
    }

    public TestableServer UseTokenSource(CancellationTokenSource tokenSource)
    {
      TokenSource = tokenSource;
      
      return this;
    }

    public void Run(int time = 3000)
    {
      var watcherTask = Task.Delay(time, TokenSource.Token)
        .ContinueWith(task => {
          if (!TokenSource.Token.IsCancellationRequested) {
            TokenSource.Cancel();
            throw new Exception("test server timeout");
          }
        });

      var hostTask = Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(builder => {
          builder.UseUrls("http://*:0").Configure(_configurer);
        })
        .Build()
        .RunAsync(TokenSource.Token);

      try {
        Task.WaitAll(new Task[] { hostTask, watcherTask });
      } catch (TaskCanceledException) {}
    }
  }
}
