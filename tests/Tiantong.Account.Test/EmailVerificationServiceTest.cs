using System.Net.Http;
using NUnit.Framework;
using Renet;
using Tiantong.Account.Utils;

namespace Tiantong.Account.Test
{
  [TestFixture]
  public class EmailVerificationServiceTest
  {
    private string _address = "627852262@qq.com";

    private EmailVerificationService GetService()
    {
      return new EmailVerificationService(new HttpClient());
    }

    [Test]
    public void TestSendSuccess()
    {
      var service = GetService();

      service.SendAsync(_address, "test", 300).GetAwaiter().GetResult();
    }

    [Test]
    public void TestSendError()
    {
      var service = GetService();

      try {
        service.SendAsync("error", "error", 300).GetAwaiter().GetResult();
      } catch (KnownException ex) {
        Assert.AreEqual(422, ex.ErrorCode);
      }
    }

    [Test]
    public void TestVerifyNotFound()
    {
      var service = GetService();

      try {
        service.VerifyAsync(_address, "1", "1234").GetAwaiter().GetResult();
      } catch (KnownException ex) {
        Assert.AreEqual(404, ex.ErrorCode);
      }
    }

    [Test]
    public void TestVerifyUnauthorized()
    {
      var service = GetService();
      var key = service.SendAsync(_address, "test").GetAwaiter().GetResult();

      try {
        service.VerifyAsync(_address, key, "e").GetAwaiter().GetResult();
      } catch (KnownException ex) {
        Assert.AreEqual(401, ex.ErrorCode);
      }
    }

    [Test]
    public void TestVerifyForbidden()
    {
      var service = GetService();
      var key = service.SendAsync(_address).GetAwaiter().GetResult();

      System.Console.WriteLine(key);

      for (var i = 0; i < 4; i++) {
        try {
          service.VerifyAsync(_address, key, "e").GetAwaiter().GetResult();
        } catch (KnownException ex) {
          Assert.AreEqual(401, ex.ErrorCode);
        }
      }

      try {
        service.VerifyAsync(_address, key, "e").GetAwaiter().GetResult();
      } catch (KnownException ex) {
        Assert.AreEqual(403, ex.ErrorCode);
      }
    }
  }
}
