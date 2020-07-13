using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using System.Threading.Tasks;
using Tiantong.Account.Utils;

namespace Tiantong.Iot.Api
{
  public class SystemController: BaseController
  {
    private Mail _mail;

    private PasswordService _passwordService;

    private SystemRepository _systemRepository;

    public SystemController(
      Mail mail,
      PasswordService passwordService,
      SystemRepository systemRepository
    ) {
      _mail = mail;
      _passwordService = passwordService;
      _systemRepository = systemRepository;
    }

    public class SystemLockParams
    {
      public string password { get; set; }
    }

    [HttpPost]
    [Route("/system-lock/get")]
    public bool GetIsSystemLocked()
    {
      return _systemRepository.GetIsSystemLocked();
    }

    [HttpPost]
    [Route("/system-lock/lock")]
    public async Task<object> LockSystem(
      [FromHeader] string authorization,
      [FromBody] SystemLockParams param
    ) {
      await _passwordService.ConfirmAsync(authorization, param.password);
      _systemRepository.LockSystem();

      return SuccessOperation("设备锁定已开启");
    }

    [HttpPost]
    [Route("/system-lock/unlock")]
    public async Task<object> UnlockSystem(
      [FromHeader] string authorization,
      [FromBody] SystemLockParams param
    ) {
      await _passwordService.ConfirmAsync(authorization, param.password);
      _systemRepository.UnlockSystem();

      return SuccessOperation("设备锁定已关闭");
    }

    [HttpPost]
    [Route("/autorun/get")]
    public object GetIsAutorun()
    {
      return _systemRepository.GetIsAutorun();
    }

    public class SetAutorunParams
    {
      public bool value { get; set; }
    }

    [HttpPost]
    [Route("/autorun/set")]
    public object SetAutorun([FromBody] SetAutorunParams param)
    {
      _systemRepository.SetAutorun(param.value);

      return SuccessOperation("自动运行设置已修改");
    }
  }
}
