using System;
using System.Linq;
using Renet.Web;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Wms.Api
{
  public class UserRepository : Repository<User, int>
  {
    private IHash _hash;

    protected DbSet<User> Users { get => DbContext.Users; }

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

    // select

    public User FindByEmail(string email)
    {
      try {
        return Users.Where(user => user.email == email).First();
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
      return Users.OrderBy(user => user.id).ToArray();
    }

    public bool HasId(int id)
    {
      return Users.Any(user => user.id == id);
    }

    public bool HasEmail(string email)
    {
      return Users.Any(user => user.email == email);
    }

    public bool HasRoot()
    {
      return Users.Any(user => user.type == "root");
    }

    public bool MatchUserPassword(User user, string password)
    {
      return _hash.Match(password, user.password);
    }
  }
}
