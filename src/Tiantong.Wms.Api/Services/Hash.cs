using System;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Tiantong.Wms.Api
{
  public class Hash : IHash
  {
    private byte[] _salt;

    private IRandom _random;

    public Hash(Config config, IRandom random)
    {
      _random = random;
      _salt = Encoding.ASCII.GetBytes(config.APP_KEY);
    }

    public string Make(string content)
    {
      var data = KeyDerivation.Pbkdf2(content, _salt, KeyDerivationPrf.HMACSHA256, 12, 256 / 8);

      return Convert.ToBase64String(data);
    }

    public bool Match(string content, string hashedContent)
    {
      return Make(content) == hashedContent;
    }
  }
}
