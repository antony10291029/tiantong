using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;

namespace Midos.Utils
{
  public class Hash : IHash
  {
    private byte[] _salt;

    private IRandom _random;

    public Hash(string secret)
    {
      _random = new Random();
      _salt = Encoding.ASCII.GetBytes(secret);
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
