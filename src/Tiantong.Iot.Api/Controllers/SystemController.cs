using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Iot.Api
{
  public class SystemController: BaseController
  {
    private SystemRepository _systemRepository;

    public SystemController(SystemRepository systemRepository)
    {
      _systemRepository = systemRepository;
    }

    [HttpPost]
    [Route("system-password/has")]
    public bool HasPassword()
    {
      return _systemRepository.HasPassword();
    }

    public class SetPasswordParams
    {
      public string password { get; set; }

      public string password_confirmation { get; set; }
    }

    [HttpPost]
    [Route("system-password/set")]
    public object SetPassword([FromBody] SetPasswordParams param)
    {
      _systemRepository.SetPassword(param.password, param.password);

      return SuccessOperation("系统密码设置完成");
    }

    public class ResetPasswordParams
    {
      public string old_password { get; set; }

      public string password { get; set; }

      public string password_confirmation { get; set; }
    }

    [HttpPost]
    [Route("system-password/reset")]
    public object ResetPassword([FromBody] ResetPasswordParams param)
    {
      _systemRepository.ResetPassword(param.old_password, param.password, param.password_confirmation);

      return SuccessOperation("密码重制成功");
    }

  }
}
