using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;

namespace Midos.SeedWork.Services
{
  public interface IHash
  {
    string Make(string content);

    bool Match(string content, string hashedContent);
  }

  public class Hash : IHash
  {
    private readonly byte[] _salt;

    public Hash(string secret)
    {
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
