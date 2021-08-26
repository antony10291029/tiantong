using Midos.Domain;
using Midos.App;
using System;
using System.Linq;

namespace Midos.Account.Api
{
  public class RoleDomainService
  {
    private readonly AppContext _context;

    public RoleDomainService(AppContext context)
    {
      _context = context;
    }

    public void Add(Role entity)
    {
      var role = Role.FromEntity(entity);

      if (_context.Any<Role>(role => role.Name == entity.Name)) {
        throw new KnownException("角色名称已存在");
      }

      _context.Add(role);
      _context.SaveChanges();
    }

    public void Update(Role entity)
    {
      var role = _context.Find<Role>(entity.Id);

      if (role == null) {
        throw new KnownException("用户不存在");
      }

      if (_context.AnyOther(role, role => role.Name == entity.Name)) {
        throw new KnownException("角色名称已存在");
      }

      _context.Entry(role).CurrentValues.SetValues(entity);
      _context.SetModified(role, role => role.CreatedAt, false);
      _context.SetModified(role, role => role.IsDeletable, false);
      _context.SaveChanges();
    }

    public void Remove(long id)
    {
      var role = _context.Find<Role>(id);

      if (role == null) {
        throw new KnownException("角色名称不存在");
      }

      _context.Remove(role);
      _context.SaveChanges();
    }

    public DataMap<Role> Query(QueryParams param)
    {
      var query = _context.AsQueryable<Role>();

      query = query.Where(role => role.Name.Contains(param.Query));

      return query.ToDataMap();
    }
  }
}
