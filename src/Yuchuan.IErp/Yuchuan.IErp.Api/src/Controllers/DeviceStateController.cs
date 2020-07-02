using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Renet.Web;
using System.Threading.Tasks;

namespace Yuchuan.IErp.Api
{
  public class DeviceStateController: BaseController
  {
    private Auth _auth;

    private DomainContext _domain;

    private DeviceStateRepository _deviceStateRepository;

    private IDistributedCache _cache;

    public DeviceStateController(
      Auth auth,
      DomainContext domain,
      DeviceStateRepository deviceStateRepository,
      IDistributedCache cache
    ) {
      _auth = auth;
      _cache = cache;
      _domain = domain;
      _deviceStateRepository = deviceStateRepository;
    }

    public class CommitParams
    {
      public int device_id { get; set; }

      public string key { get; set; }

      public string value { get; set; }
    }

    [HttpPost]
    [Route("/device-states/commit")]
    public async Task<ActionResult<object>> Commit([FromBody] CommitParams param)
    {
      await _deviceStateRepository.CommitAsync(param.device_id, param.key, param.value);

      return new {
        message = "状态已提交"
      };
    }
  }
}
