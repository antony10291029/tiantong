using System.Text;
using System;

namespace Tiantong.Wms.Api
{
  public class Random : IRandom
  {
    private System.Random _random;

    public Random()
    {
      _random = new System.Random((int)DateTime.Now.Ticks);
    }

    public string String(int length)
    {
      var builder = new StringBuilder();
      char ch;
      for (int i = 0; i < length; i++)
      {
        ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65)));                 
        builder.Append(ch);
      }

      return builder.ToString();
    }
  }
}
