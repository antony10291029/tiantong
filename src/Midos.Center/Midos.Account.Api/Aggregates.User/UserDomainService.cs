using Midos.App;
using Midos.Domain;
using System;
using System.Linq;

namespace Midos.Account.Api
{
  public interface IUserDomainService
  {
    void Add(User entity);

    void Update(User entity);

    void Delete(long id);

    User Find(long id);

    DataMap<User> Query(QueryParams param);

    DataPagination<User> Paginate(PaginateParams param);
  }

  public class UserDomainService
  {
    private readonly AppContext _context;

    public UserDomainService(AppContext context)
    {
      _context = context;
    }

    public void Add(User entity)
    {
      var user = User.FromEntity(entity);

      if (_context.Set<User>().Any(user => user.Email == entity.Email)) {
        throw new KnownException("邮箱已存在");
      }

      if (_context.Set<User>().Any(user => user.Mobile == entity.Mobile)) {
        throw new KnownException("手机号已存在");
      }

      if (_context.Set<User>().Any(user => user.Username == entity.Username)) {
        throw new KnownException("用户名已存在");
      }

      _context.Add(user);
      _context.SaveChanges();
    }

    public void Update(User entity)
    {
      var user = _context.Find<User>(entity.Id);

      if (user == null) {
        throw new KnownException("用户不存在");
      }

      if (_context.AnyOther(user, user => user.Email == entity.Email)) {
        throw new KnownException("邮箱已存在");
      }

      if (_context.AnyOther(user, user => user.Mobile == entity.Mobile)) {
        throw new KnownException("手机号已存在");
      }

      if (_context.AnyOther(user, user => user.Username == entity.Username)) {
        throw new KnownException("用户名已存在");
      }

      _context.Entry(user).CurrentValues.SetValues(entity);
      _context.SetModified(user, user => user.Password, false);
      _context.SetModified(user, user => user.IsDeletable, false);
      _context.SetModified(user, user => user.CreatedAt, false);
      _context.SaveChanges();
    }

    public void Delete(long id)
    {
      var user = _context.Set<User>().Find(id);

      if (!user.IsDeletable) {
        throw new KnownException("该用户不可删除");
      }

      _context.Remove(user);
      _context.SaveChanges();
    }

    public User Find(long id)
    {
      return _context.Set<User>().Find(id);
    }

    public DataMap<User> Query(QueryParams param)
    {
      var query = _context.AsQueryable<User>();

      query = query.Where(user =>
        user.Email.Contains(param.Query) ||
        user.Mobile.Contains(param.Query) ||
        user.Username.Contains(param.Query)
      );

      return query.ToDataMap();
    }

    public DataPagination<User> Paginate(PaginateParams param)
    {
      var query = _context.AsQueryable<User>();

      query = query.Where(user =>
        user.Email.Contains(param.Query) ||
        user.Mobile.Contains(param.Query) ||
        user.Username.Contains(param.Query)
      );

      return query.Paginate(param);
    }
  }
}
