using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Midos.Domain.Test
{
  public class DomainContextTest
  {

    [TestMethod]
    public void TestDomainContext()
    {
      using var domain = new TestDomainContext();

      domain.Users.Add(new User("foo"));
      domain.Users.Add(new User("bar"));

      domain.SaveChanges();

      var users = domain.Set<User>().ToArray();

      Assert.AreEqual(2, users.Length);
    }

  }
}