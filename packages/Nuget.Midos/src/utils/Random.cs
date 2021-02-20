using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Midos.Utils
{
  public class Random : IRandom
  {
    private System.Random _random;

    public Random()
    {
      _random = new System.Random((int) System.DateTime.Now.Ticks);
    }

    public bool Bool()
    {
      return Int(0, 1) == 0 ? false : true;
    }

    public T Array<T>(T[] array)
    {
      return array[Int(0, array.Length - 1)];
    }

    public T[] Array<T>(T[] array, int count)
    {
      return array.OrderBy(item => _random.Next()).Take(count).ToArray();
    }

    public T[] Array<T>(T[] array, int min, int max)
    {
      return Array(array, Int(min, max));
    }

    public int Int(int min, int max)
    {
      return _random.Next(min, max + 1);
    }

    public string String(int length)
    {
      var builder = new StringBuilder();
      for (int i = 0; i < length; i++) {
        var str = Int(0, 35);
        str += (str < 10 ? '0' : ('A' - 10));
        builder.Append((char) str);
      }

      return builder.ToString();
    }

    public Action<Action<int>> For(int min, int max)
    {
      var n = Int(min, max);

      return callback => {
        for (var i = 1; i <= n; i++) {
          callback(i);
        }
      };
    }

    public void For(int min, int max, Action<int> callback)
    {
      var n = Int(min, max);

      for (var i = 1; i <= n; i++) {
        callback(i);
      }
    }

    public IEnumerable<int> Enumerate(int min, int max)
    {
      return Enumerable.Range(0, Int(min, max));
    }

    public DateTime DateTime(DateTime min, DateTime max)
    {
      var range = (max - min).Days;

      return min.AddDays(_random.Next(range));
    }
  }
}
