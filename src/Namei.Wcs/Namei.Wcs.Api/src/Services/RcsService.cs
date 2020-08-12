using System.Linq;
using System.Net.Mime;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Namei.Wcs.Api
{
  public class RcsService
  {
    private HttpClient _client;

    private DomainContext _domain;

    public RcsService(IHttpClientFactory factory, DomainContext domain)
    {
      _domain = domain;
      _client = factory.CreateClient();
      _client.BaseAddress = new System.Uri("http://172.16.2.94:8000");
      _client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json)
      );
    }

    private void Send(string doorId, string uuid, string action)
    {
      var json = JsonSerializer.Serialize(new {
        deviceType = "door",
        deviceIndex = doorId,
        actionStatus = action,
        uuid = uuid,
      });
      var content = new StringContent(json, Encoding.UTF8);
      _client.PostAsync("cms/services/rest/liftCtlService/notifyExcuteResultInfo", content)
        .GetAwaiter().GetResult();
    }

    public void NotifyDoorOpened(string doorId)
    {
      var tasks = _domain.RcsTasks.Where(task =>
        task.type == "notifyTask" &&
        task.action_task == "applyLock" &&
        task.device_type == "door" &&
        task.device_index == doorId
      ).ToArray();

      foreach (var task in tasks) {
        Send(task.device_index, task.uuid, "1");
      }

      _domain.RemoveRange(tasks);
      _domain.SaveChanges();
    }

    public void NotifyDoorClosed(string doorId)
    {
      var tasks = _domain.RcsTasks.Where(task =>
        task.type == "notifyTask" &&
        task.action_task == "releaseDevice" &&
        task.device_type == "door" &&
        task.device_index == doorId
      ).ToArray();

      foreach (var task in tasks) {
        Send(task.device_index, task.uuid, "2");
      }

      _domain.RemoveRange(tasks);
      _domain.SaveChanges();
    }
  }
}
