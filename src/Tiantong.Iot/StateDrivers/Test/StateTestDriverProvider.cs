namespace Tiantong.Iot
{
  public class StateTestDriverProvider : IStateDriverProvider
  {
    public IStateDriver Resolve()
    {
      var client = new StateTestDriver();
      client.Store = new StateTestDriverStore();

      return client;
    }

    public void Boot()
    {

    }
  }
}
