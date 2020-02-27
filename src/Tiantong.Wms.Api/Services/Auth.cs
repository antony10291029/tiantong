using System;
using System.Linq;
using System.Text.Json;
using Tiantong.Wms.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using JWT.Builder;
using JWT.Algorithms;

namespace Tiantong.Wms.Api
{
  public class Auth : IAuth
  {
    private PostgresContext _db;

    private int _ttl;

    private int _rft;

    private string _secret;

    private string _appName;

    private User _user;

    public User User
    {
      get {
        if (_user == null) {
          _user = ResolveUser();
        }

        return _user;
      }
    }

    public IHttpContextAccessor _httpAccessor;

    public Auth(PostgresContext db, IHttpContextAccessor httpAccessor, IConfiguration config)
    {
      _db = db;
      _httpAccessor = httpAccessor;
      _secret = config["jwt:key"];
      _appName = config["app_name"];
      _ttl = config.GetValue<int>("jwt:ttl");
      _rft = config.GetValue<int>("jwt:rft");
    }

    public void Ensure()
    {
      EnsureRoles();
    }

    public void EnsureRoles(params string[] roles)
    {
      _user = ResolveUser();
      if (roles.Length != 0 && !roles.Any(role => User.roles.Contains(role))) {
        throw new AuthException("Authorization role is not match");
      }
    }

    public (string, DateTime, DateTime) Encode(User user)
    {
      var iat = DateTime.Now;
      var exp = iat.AddSeconds(_ttl);
      var rfa = iat.AddSeconds(_rft);
      var token = "Bearer " + new JwtBuilder()
        .WithAlgorithm(new HMACSHA256Algorithm())
        .WithSecret(_secret)
        .AddClaim("user_id", user.id)
        .AddClaim("app_name", _appName)
        .AddClaim("exp", exp)
        .AddClaim("rfa", rfa)
        .Encode();

      return (token, exp, rfa);
    }

    public User ResolveUser()
    {
      var token = _httpAccessor.HttpContext.Request.Headers["Authorization"].ToString();

      if (token == "" || token == null) {
        throw new AuthException("Authorization field is empty");
      }
      if (token.Length <= 7) {
        throw new AuthException("Authorization fiels length must bigger than 7");
      }
      if (token.Substring(0, 7) != "Bearer ") {
        throw new AuthException("Authorization token must start with `Bearer `");
      }
      token = token.Substring(7);
      var data = "";

      try {
        data = new JwtBuilder()
          .WithAlgorithm(new HMACSHA256Algorithm())
          .WithSecret(_secret)
          .Decode(token);
      } catch {
        throw new AuthException("unable to decode Authorization token");
      }

      var json = JsonDocument.Parse(data);
      var exp = json.RootElement.GetProperty("exp").GetDateTime();
      var rfa = json.RootElement.GetProperty("rfa").GetDateTime();
      var now = DateTime.Now;
      if (exp <= now) {
        throw new AuthException("Authorization token is expired");
      }

      var appName = json.RootElement.GetProperty("app_name").GetString();
      if (appName != _appName) {
        throw new AuthException("Authorization application is invalid");
      }

      var id = json.RootElement.GetProperty("user_id").GetInt32();
      var user = _db.Users.Where(item => item.id == id).FirstOrDefault();
      if (user == null) {
        throw new AuthException("Authorization user is not existed");
      }

      return user;
    }
  }
}
