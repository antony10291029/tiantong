using System.Linq;
using NUnit.Framework;

namespace DBCore.Sqlite.Test
{
  [TestFixture]
  public class TestMigrator
  {
    [Test]
    public void TestMigrateDefaultDirectory()
    {
      var db = new Database();
      var mg = new Migrator(db);

      Assert.AreEqual(2, mg.Migrate());
      Assert.AreEqual(0, mg.Migrate());
      Assert.IsTrue(db.HasTable("users"));
      Assert.IsTrue(db.HasTable("roles"));
      Assert.IsFalse(db.HasTable("foo"));

      db.Dispose();
    }

    [Test]
    public void TestRollback()
    {
      var db = new Database();
      var mg = new Migrator(db);

      mg.Migrate();
      Assert.AreEqual(2, mg.Rollback());
      Assert.AreEqual(0, mg.Rollback());
      Assert.IsFalse(db.HasTable("users"));

      db.Dispose();
    }

    [Test]
    public void TestRefresh()
    {
      var db = new Database();
      var mg = new Migrator(db);

      mg.Migrate();
      mg.Refresh();

      db.Users.ToList();
      Assert.IsTrue(db.HasTable("users"));

      db.Dispose();
    }

    [Test]
    public void TestUseMigrationDirectory()
    {
      var db = new Database();
      var mg = new Migrator(db);
      var count = mg.UseMigrationDirectory("MigrationsFoo").Migrate();

      Assert.AreEqual(1, count);
      Assert.IsTrue(db.HasTable("foo"));
      Assert.IsFalse(db.HasTable("users"));

      db.Dispose();
    }

    [Test]
    public void TestMigrationOrder()
    {
      var db = new Database();
      var mg = new Migrator(db);

      mg.UseMigrationDirectory("MigrationsBar").Migrate();

      Assert.IsTrue(db.HasTable("bar"));

      mg.Rollback();

      db.Dispose();
    }

  }
}
