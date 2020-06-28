using Renet.Web;

namespace Tiantong.Account.Api
{
  public class JWT: Auth
  {
    public JWT(Config config): base(config.JWT_SECRET, config.JWT_TTL, config.JWT_RFT)
    {

    }
  }
}
