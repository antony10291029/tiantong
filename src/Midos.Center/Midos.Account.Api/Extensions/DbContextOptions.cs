using Microsoft.EntityFrameworkCore;
using Midos.Domain;
using System.Linq.Expressions;

namespace System.Linq
{
  public static class DbContextExtensions
  {
    public static IQueryable<TEntity> AsQueryable<TEntity>(this DbContext context)
      where TEntity: class =>
      context.Set<TEntity>().AsQueryable();

    public static bool Any<TEntity>(this DbContext context, Expression<Func<TEntity, bool>> predicate)
      where TEntity: class =>
      context.Set<TEntity>().Any(predicate);

    public static bool AnyOther<TEntity>(this DbContext context, TEntity entity, Expression<Func<TEntity, bool>> predicate)
      where TEntity: class, IEntity =>
      context.Set<TEntity>().Where(item => item.Id != entity.Id).Any(predicate);

    public static void SetModified<TEntity, TProperty>(
      this DbContext context, TEntity entity,
      Expression<Func<TEntity, TProperty>> expression, bool value
    ) where TEntity: class =>
      context.Entry(entity).Property(expression).IsModified = value;
  }
}
