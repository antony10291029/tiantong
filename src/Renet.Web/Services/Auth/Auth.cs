using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Text.Json;

namespace Renet.Web
{
  public class Auth
  {
    private long _ttl;

    private long _rft;

    private string _secret;

    //

    public Auth(string secret, long ttl, long rft)
    {
      _secret = secret;
      _ttl = ttl;
      _rft = rft;
    }

    public (string, DateTime, DateTime) Encode(int id, long? ttl = null, long? rft = null)
    {
      var iat = DateTime.Now;
      var exp = iat.AddSeconds(ttl ?? this._ttl);
      var rfa = iat.AddSeconds(rft ?? this._rft);
      var token = "Bearer " + new JwtBuilder()
        .WithAlgorithm(new HMACSHA256Algorithm())
        .WithSecret(_secret)
        .AddClaim("id", id)
        .AddClaim("exp", exp)
        .AddClaim("rfa", rfa)
        .Encode();

      return (token, exp, rfa);
    }

    public (int, DateTime, DateTime) Decode(string token)
    {
      if (token == "" || token == null) {
        throw KnownException.Error("token 不能为空", 401);
      }
      if (token.Length <= 7) {
        throw KnownException.Error("token 长度错误", 401);
      }
      if (token.Substring(0, 7) != "Bearer ") {
        throw KnownException.Error("token 必须由 `Bearer ` 开头", 401);
      }
      token = token.Substring(7);

      try {
        var data = new JwtBuilder()
          .WithAlgorithm(new HMACSHA256Algorithm())
          .WithSecret(_secret)
          .Decode(token);
        var json = JsonDocument.Parse(data);

        return (
          json.RootElement.GetProperty("id").GetInt32(),
          json.RootElement.GetProperty("exp").GetDateTime(),
          json.RootElement.GetProperty("rfa").GetDateTime()
        );
      } catch (KnownException e) {
        throw e;
      } catch {
        throw KnownException.Error("token 解析失败", 401);
      }
    }

    public (int, DateTime, DateTime) Parse(string token)
    {
      var (id, exp, rfa) = Decode(token);

      if (exp < DateTime.Now) {
        throw KnownException.Error("token 已过期", 403);
      }

      return (id, exp, rfa);
    }
  }
}
