using System;

namespace Tiantong.Wms.Api
{
  public interface IRandom
  {
    bool Bool();

    int Int(int min, int max);

    string String(int length);

    DateTime DateTime(DateTime min, DateTime max);
  }
}
