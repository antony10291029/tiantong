using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Account.Api
{
  [Route("/users")]
  public class UserController: BaseController
  {
    private AccountContext _context;

    public UserController(
      AccountContext context
    ) {
      _context = context;
    }

    public ActionResult<object> RegisterByEmail()
    {
      return new {
        message = ""
      };
    }

  }
}
