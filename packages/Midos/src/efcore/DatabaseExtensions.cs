using System;

namespace Microsoft.EntityFrameworkCore
{
  public static class DbContextExtensions
  {
    public static void UseTransaction(this DbContext dbContext, Action handler, Action<Exception> error)
    {
      try {
        var transaction = dbContext.Database.BeginTransaction();
        handler();
        transaction.Commit();
      } catch (Exception e) {
        error(e);
        throw e;
      }
    }
  }
}
