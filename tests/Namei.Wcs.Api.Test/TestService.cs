using Namei.Wcs.Aggregates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Namei.Wcs.Api.Test
{
  [TestClass]
  public class TestService
  {
    [TestMethod]
    public void Test()
    {
      using var domain = Utils.GetDomain();

      for (var i = 0; i < 10; i++) {
        domain.Add(RcsAgcTask.From(
          taskType: "asdf",
          position: "asdf",
          destination: "fasdf",
          podCode: "asdf",
          comment: "asdf",
          orderType: "asdf",
          orderId: 100
        ));
      }

      domain.SaveChanges();
    }
  }
}
