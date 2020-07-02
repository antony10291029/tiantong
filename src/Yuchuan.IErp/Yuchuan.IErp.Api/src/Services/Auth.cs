using Renet;
using Tiantong.Account.Utils;
using Microsoft.AspNetCore.Http;

namespace Yuchuan.IErp.Api
{
  public class Auth
  {
    private TokenService _tokenService;

    private HttpContext _httpContext;

    private string _token
    {
      get => _httpContext.Request.Headers["Authorization"];
    }

    public Auth(
      TokenService tokenService,
      IHttpContextAccessor accessor
    ) {
      _tokenService = tokenService;
      _httpContext = accessor.HttpContext;
    }

    public int GetUserId()
    {
      return _tokenService.VerifyAsync(_token).GetAwaiter().GetResult();
    }
  }
}
