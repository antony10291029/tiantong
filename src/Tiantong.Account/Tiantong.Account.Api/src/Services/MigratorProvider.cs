using DBCore;
using Tiantong.Account.Database;

namespace Tiantong.Account.Api
{
  public class MigratorProvider
  {
    public IMigrator Migrator { get; }

    public MigratorProvider(AccountContext context)
    {
      Migrator = new PostgresMigrator(context);
    }
  }
}
