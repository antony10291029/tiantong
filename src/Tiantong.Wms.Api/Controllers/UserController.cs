using Renet.Web;
using Tiantong.Wms.DB;
using System.Linq;

namespace Tiantong.Wms.Api
{
  public class UserController : BaseController
  {
    public object Search()
    {
      var db = new PostgresContext();

      return db.Users.ToList();
    }

    public object Create()
    {
      return new {
        message = "success to create user",
      };
    }
  }
}
