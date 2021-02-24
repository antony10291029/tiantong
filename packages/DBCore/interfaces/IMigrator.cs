namespace DBCore
{
  public interface IMigrator
  {
    int Migrate();

    int Rollback();

    int Refresh();
  }
}
