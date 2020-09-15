using DotNetCore.CAP;
using System;
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

    private ICapPublisher _cap;

    public RcsService(IHttpClientFactory factory, ICapPublisher cap)
    {
      _cap = cap;
      _client = factory.CreateClient();
      _client.Timeout = new TimeSpan(0, 0, 10);
      _client.BaseAddress = new System.Uri("http://172.16.2.230:80");
      _client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json)
      );
    }

    private void Send(string doorId, string uuid, string action)
    {
      if (uuid == "" || uuid == "A001") {
        return;
      }

      var json = JsonSerializer.Serialize(new {
        deviceType = "door",
        deviceIndex = doorId,
        actionStatus = action,
        uuid = uuid,
      });
      var content = new StringContent(json, Encoding.UTF8);

      try {
        var response = _client.PostAsync("/rcs/services/rest/liftCtlService/notifyExcuteResultInfo", content).GetAwaiter().GetResult();
        var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        _cap.Publish(RcsNotifiedEvent.Message, new RcsNotifiedEvent(doorId, action, uuid, result));
      } catch (Exception e) {
        _cap.Publish(RcsNotifiedFailedEvent.Message, new RcsNotifiedFailedEvent(doorId, action, uuid, e.Message));
      }
    }

    public void NotifyDoorOpened(string doorId, string uuid)
      => Send(doorId, uuid, "1");

    public void NotifyDoorClosing(string doorId, string uuid)
      => Send(doorId, uuid, "2");
  }
}
