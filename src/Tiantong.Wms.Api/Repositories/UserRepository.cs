using System;
using System.Linq;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class UserRepository : Repository<User, int>
  {
    private IHash _hash;

    public IQueryable<User> Owners
    {
      get => Table.Where(user => user.type == UserTypes.Owner)
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

    // delete



    // update

    public override User Update(User user)
    {
      return base.Update(user);
    }

    public void EncodePassword(User user)
    {
      user.password = _hash.Make(user.password);
    }

    // ensure

    public void EnsureUnique(User user)
    {
      if (
        Table.Any(item => 
          item.id != user.id && item.email == user.email
        )
      ) {
        throw new FailureOperation("用户邮箱重复");
      }
    }

    // select

    public User FindByEmail(string email)
    {
      try {
        return Table.Where(user => user.email == email).First();
      } catch (Exception e) {
        if (e.Message == "Sequence contains no elements") {
          throw new HttpException("Fail to find user by email");
        } else {
          throw e;
        }
      }
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
