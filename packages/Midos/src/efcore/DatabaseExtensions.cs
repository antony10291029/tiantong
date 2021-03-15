using System;

namespace Microsoft.EntityFrameworkCore
{
  public static class DbContextExtensions
  {
    public static void UseTransaction(this DbContext dbContext, Action handler, Action<Exception> error = null)
    {
      try {
        var transaction = dbContext.Database.BeginTransaction();
        handler();
        transaction.Commit();
      } catch (Exception e) {
        if (error != null) {
          error(e);
        }

        throw e;
      }
    }

    public static void SaveChanges(this DbContext dbContext, Action handler, Action<Exception> error = null)
      => UseTransaction(
        dbContext,
        () => {
          dbContext.SaveChanges();
          handler();
        },
        error
      );
  }
}
