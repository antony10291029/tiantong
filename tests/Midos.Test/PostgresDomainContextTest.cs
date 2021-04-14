using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Midos.Domain.Test.PostgresOptions
{
  [TestClass]
  public class PostgresOptionsTest
  {
    [ClassInitialize]
    public static void Initialize(TestContext ctx)
    {
      using var domain = new TestDomainContext();

      domain.Database.EnsureCreated();
    }

    [ClassCleanup]
    public static void Cleanup()
    {
      using var domain = new TestDomainContext();

      domain.Database.EnsureDeleted();
    }

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

  public class User
  {
    public long Id { get; private set; }

    public string Name { get; private set; }

    public User(string name)
    {
      Name = name;
    }
  }

  public class TestDomainContext: DomainContext
  {
    public DbSet<User> Users { get; set; }

    public TestDomainContext(): base(
      new PostgresDomainOptions<TestDomainContext>(),
      new TestEventPublisher()
    ) {

    }
  }
}
