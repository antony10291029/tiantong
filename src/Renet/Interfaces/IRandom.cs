using System;
using System.Collections.Generic;

namespace Renet
{
  public interface IRandom
  {
    bool Bool();

    T Array<T>(T[] arr);

    T[] Array<T>(T[] arr, int count);

    T[] Array<T>(T[] array, int min, int max);

    int Int(int min, int max);

    string String(int length);

    Action<Action<int>> For(int min, int max);

    void For(int min, int max, Action<int> callback);

    IEnumerable<int> Enumerate(int min, int max);

    DateTime DateTime(DateTime min, DateTime max);

  }
}
