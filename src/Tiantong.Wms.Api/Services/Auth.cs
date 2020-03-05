using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using JWT.Builder;
using JWT.Algorithms;

namespace Tiantong.Wms.Api
{
  public class Auth : IAuth
  {
    private DbContext _db;

    private IHttpContextAccessor _httpAccessor;

    private User _user;

    private int _ttl;

    private int _rft;

    private string _secret;

    private string _appName;

    private bool _isTokenResolved = false;

    private bool _needToRefresh = false;

    //

    public User User
    {
      get {
        if (_user == null) {
          _user = ResolveToken();
        }

        return _user;
      }
    }

    public bool NeedToRefresh
    {
      get {
        if (!_isTokenResolved) {
          ResolveToken();
        }

        return _needToRefresh;
      }
    }

    public Auth(DbContext db, IHttpContextAccessor httpAccessor, IConfiguration config)
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
      if (User == null) {
        throw new AuthException("Authorization user is invalid");
      }
    }

    public void EnsureType(string type)
    {
      Ensure();
      if (User.type != type) {
        throw new AuthException("Authorization user type is not match");
      }
    }

    public void EnsureRoot()
    {
      EnsureType(UserTypes.Root);
    }

    public void EnsureOwner()
    {
      EnsureType(UserTypes.Owner);
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

    private User ResolveToken()
    {
      _isTokenResolved = true;

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
      if (rfa <= now) {
        _needToRefresh = true;
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
