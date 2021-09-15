using System;
using System.Linq;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore
{
  public static class DbContextExtensions
  {
    public static bool HasTable(this DbContext db, string table)
    {
      try {
        db.Database.ExecuteSqlRaw($"select 1 from \"{table}\" limit 1");
        return true;
      } catch {
        return false;
      }
    }

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

        throw;
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

    public static TEntity First<TEntity>(this DbContext dbContext, Expression<Func<TEntity, bool>> predicate)
      where TEntity: class
      => dbContext.Set<TEntity>().First(predicate);
  }
}
