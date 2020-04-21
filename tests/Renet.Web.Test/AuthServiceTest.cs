using System.Threading.Tasks;
using System;
using NUnit.Framework;

namespace Renet.Web.Test
{
  [TestFixture]
  public class AuthServiceTest
  {
    private string _secret = "rPMfnL2Y4cvMEYd2J4e2OhrbLrg0qmDk";

    [TestCase(0, 0)]
    [TestCase(1, 1)]
    [TestCase(10, 120)]
    [TestCase(-10, -120)]
    public void TestEncode(int ttl, int rft)
    {
      var auth = new Auth(_secret, ttl, rft);
      var before = DateTime.Now.AddMilliseconds(-10);
      var (token, expiredAt, refreshAt) = auth.Encode(100);
      var after = DateTime.Now.AddMilliseconds(10);

      Assert.True(before.AddSeconds(ttl) < expiredAt);
      Assert.True(before.AddSeconds(rft) < refreshAt);
      Assert.True(DateTime.Now.AddSeconds(ttl) > expiredAt);
      Assert.True(DateTime.Now.AddSeconds(rft) > refreshAt);
    }

    [TestCase(1, 10)]
    [TestCase(10, 100)]
    [TestCase(10, -100)]
    public void TestDecode(int id, int ttl)
    {
      var auth = new Auth(_secret, ttl, 0);
      var (token, expiredAt, refreshAt) = auth.Encode(1);
      token = $"Bearer " + token;

      try {
        (id, _, _) = auth.Decode(token);
        if (expiredAt < DateTime.Now) {
          Assert.Fail("expect throw exception when auth token is expired");
        }
      } catch (Exception) {
        if (expiredAt > DateTime.Now) {
          Assert.Fail("unexpected exception because token is valid");
        }
      }
    }
  }
}
