namespace Tiantong.Account.Api
{
  public class Hash: Renet.Utils.Hash
  {
    public Hash(Config config): base(config.AppKey)
    {

    }
  }
}
