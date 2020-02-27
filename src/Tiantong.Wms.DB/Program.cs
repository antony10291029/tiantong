namespace Tiantong.Wms.DB
{
  public static class Program
  {
    public static void Main(string[] argv)
    {
      var migrator = new PostgresMigrator();

      migrator.Refresh();

      var db = new PostgresContext();

      db.Users.Add(new User {
        email = "zhanglan",
        password = "aeoikj",
        roles = new string[] { "admin", "operator" },
        name = "aeoikj",
      });

      db.SaveChanges();
    }
  }
}
