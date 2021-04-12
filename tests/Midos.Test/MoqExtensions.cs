using System;

namespace Midos.Test
{
  public class Mock
  {
    public static TService UseService<TService>(Action<Moq.Mock<TService>> useMock = null)
      where TService: class
    {
      var mock = new Moq.Mock<TService>();

      if (useMock != null) {
        useMock(mock);
      }

      return mock.Object;
    }
  }
}
