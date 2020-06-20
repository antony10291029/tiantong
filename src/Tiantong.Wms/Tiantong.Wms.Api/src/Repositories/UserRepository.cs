using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class UserRepository : Repository<User, int>
  {
    private IHash _hash;

    public IQueryable<User> Owners
    {
      get => Table.Where(user => user.type == UserType.User)
        .OrderBy(user => user.id);
    }

    public UserRepository(DbContext db, IHash hash) : base(db)
    {
      _hash = hash;
    }

    // insert
    public override User Add(User user)
    {
      EncodePassword(user);

      return base.Add(user);
    }

    public User Register(User user)
    {
      var entity = new User {
        name = user.name,
        email = user.email,
        type = UserType.User,
        password = user.password,
      };

      EnsureUnique(entity);
      Add(entity);

      return entity;
    }

    // delete



    // update

    public override User Update(User user)
    {
      base.Update(user);
      DbContext.Entry(user).Property(u => u.password).IsModified = false;

      return user;
    }

    public void Update(User oldUser, User user)
    {
      DbContext.Entry(oldUser).CurrentValues.SetValues(user);
      DbContext.Entry(oldUser).Property(u => u.id).IsModified = false;
      DbContext.Entry(oldUser).Property(u => u.password).IsModified = false;
    }

    public void EncodePassword(User user)
    {
      user.password = _hash.Make(user.password);
    }

    // ensure

    public void EnsureUnique(User user)
    {
      if (
        user.id == 0 ? Table.Any(
          u => u.email == user.email
        ) : Table.Any(u =>
          u.id != user.id &&
          u.email == user.email
        )
      ) {
        throw new FailureOperation("用户邮箱已存在");
      }
    }

    // select

    public User GetByEmail(string email)
    {
      return Table.FirstOrDefault(user => user.email == email);
    }

    public User EnsureGetByEmail(string email)
    {
      var user = GetByEmail(email);

      if (user == null) {
        throw new FailureOperation("用户邮箱不存在");
      }

      return user;
    }

    public User EnsureGet(int id)
    {
      var user = Get(id);

      if (user == null) {
        throw new FailureOperation("该用户不存在");
      }

      return user;
    }

    public User[] Search()
    {
      return Table.OrderBy(user => user.id).ToArray();
    }

    public bool HasId(int id)
    {
      return Table.Any(user => user.id == id);
    }

    public bool HasEmail(string email)
    {
      return Table.Any(user => user.email == email);
    }

    public bool HasRoot()
    {
      return Table.Any(user => user.type == "root");
    }

    public bool MatchUserPassword(User user, string password)
    {
      return _hash.Match(password, user.password);
    }
  }
}
