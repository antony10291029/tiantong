using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Tiantong.Iot.Api
{
  public class SystemController: BaseController
  {
    private Mail _mail;

    private SystemRepository _systemRepository;

    public SystemController(
      Mail mail,
      SystemRepository systemRepository
    ) {
      _mail = mail;
      _systemRepository = systemRepository;
    }

    public class SystemLockParams
    {
      public string password { get; set; }
    }

    [HttpPost("/system-lock/get")]
    public bool GetIsSystemLocked()
    {
      return _systemRepository.GetIsSystemLocked();
    }

    [HttpPost("/autorun/get")]
    public object GetIsAutorun()
    {
      return _systemRepository.GetIsAutorun();
    }

    public class SetAutorunParams
    {
      public bool value { get; set; }
    }

    [HttpPost("/autorun/set")]
    public object SetAutorun([FromBody] SetAutorunParams param)
    {
      _systemRepository.SetAutorun(param.value);

      return SuccessOperation("自动运行设置已修改");
    }
  }
}
