using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class Auth : Renet.Web.Auth
  {
    private DbContext _db;

    private IHttpContextAccessor _httpAccessor;

    //

    private User _user;

    public User User
    {
      get {
        if (_user == null) {
          _user = ResolveToken();
          EnsureUser();
        }

        return _user;
      }
    }

    public Auth(
      DbContext db,
      Config config,
      IHttpContextAccessor httpAccessor
    ) : base(
      config.APP_NAME,
      config.JWT_TTL,
      config.JWT_RFT
    ) {
      _db = db;
      _httpAccessor = httpAccessor;
    }

    public void EnsureValid()
    {
      if (_user == null) {
        _user = ResolveToken();
      }
    }

    public void EnsureType(string type)
    {
      if (_user == null) {
        _user = ResolveToken();
      }

      if (_user.type != type) {
        throw new FailureOperation("用户类型错误");
      }
    }

    public void EnsureRoot()
    {
      EnsureType(UserType.Root);
    }

    public void EnsureUser()
    {
      EnsureType(UserType.User);
    }

    public (string, DateTime, DateTime) Encode(User user)
    {
      return Encode(user.id);
    }

    private User ResolveToken()
    {
      var token = _httpAccessor.HttpContext.Request.Headers["Authorization"].ToString();

      try {
        var (id, _, _) = Decode(token);
        var user = _db.Users.Where(item => item.id == id).FirstOrDefault();

        if (user == null) {
          throw new FailureOperation("用户身份已过期，请重新登陆");
        }

        return user;
      } catch (AuthException) {
        throw new FailureOperation("身份验证失败，请重新登陆");
      } catch (AuthTokenExpiredException) {
        throw new FailureOperation("登陆已超时，请重新登陆");
      }
    }
  }
}
