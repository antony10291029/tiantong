namespace Tiantong.Wms.DB
{
  public static class Program
  {
    public static void Main(string[] argv)
    {
      var migrator = new PostgresMigrator();

      // migrator.Rollback();
      // migrator.Migrate();
      migrator.Refresh();
    }
  }
}
