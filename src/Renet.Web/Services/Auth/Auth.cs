using System;
using JWT.Builder;
using JWT.Algorithms;
using System.Text.Json;

namespace Renet.Web
{
  public class Auth
  {
    private int _ttl;

    private int _rft;

    private string _secret;

    //

    public Auth(string secret, int ttl, int rft)
    {
      _secret = secret;
      _ttl = ttl;
      _rft = rft;
    }

    public (string, DateTime, DateTime) Encode(int id)
    {
      var iat = DateTime.Now;
      var exp = iat.AddSeconds(_ttl);
      var rfa = iat.AddSeconds(_rft);
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
        throw new AuthException("Authorization field is empty");
      }
      if (token.Length <= 7) {
        throw new AuthException("Authorization fiels length must bigger than 7");
      }
      if (token.Substring(0, 7) != "Bearer ") {
        throw new AuthException("Authorization token must start with `Bearer `");
      }
      token = token.Substring(7);

      try {
        var data = new JwtBuilder()
          .WithAlgorithm(new HMACSHA256Algorithm())
          .WithSecret(_secret)
          .Decode(token);

        var json = JsonDocument.Parse(data);
        var id = json.RootElement.GetProperty("id").GetInt32();
        var exp = json.RootElement.GetProperty("exp").GetDateTime();
        var rfa = json.RootElement.GetProperty("rfa").GetDateTime();

        if (exp < DateTime.Now) {
          throw new AuthTokenExpiredException("Auth token is expired");
        }

        return (id, exp, rfa);
      } catch (AuthTokenExpiredException e) {
        throw e;
      } catch {
        throw new AuthException("unable to decode Authorization token");
      }
    }
  }
}
