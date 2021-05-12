using Midos.Services.Abstract;
using System.Threading;
using System.Threading.Tasks;

namespace Namei.ApiGateway.Server
{
  public class HttpLogHostedService: IntervalService
  {
    private readonly HttpLogService _httpLogService;

    public HttpLogHostedService(HttpLogService httpLogService)
    {
      Time = 1000;
      _httpLogService = httpLogService;
    }

    protected override async Task HandleJob(CancellationToken stoppingToken)
    {
      await _httpLogService.SaveAsync();
    }
  }
}
