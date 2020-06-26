using Tiantong.Account.Database;

namespace Tiantong.Account.Api
{
  public class AccountContext: PostgresContext
  {
    public AccountContext(DbBuilder builder): base(builder)
    {

    }
  }
}
