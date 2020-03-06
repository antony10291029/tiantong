using System;

namespace Tiantong.Wms.Api
{
  public interface IRandom
  {
    bool Bool();

    T Array<T>(T[] arr);

    T[] Array<T>(T[] arr, int count);

    int Int(int min, int max);

    string String(int length);

    DateTime DateTime(DateTime min, DateTime max);

  }
}
