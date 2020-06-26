using Tiantong.Account.Database;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Account.Api
{
  public class AccountContext: PostgresContext
  {
    public DbSet<User> Users { get ; set; }

    public DbSet<EmailVerification> EmailVerifications { get; set; }

    public AccountContext(DbBuilder builder): base(builder)
    {

    }
  }
}
