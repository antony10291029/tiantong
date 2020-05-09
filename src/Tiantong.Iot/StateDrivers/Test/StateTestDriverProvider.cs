namespace Tiantong.Iot
{
  public class StateTestDriverProvider : IStateDriverProvider
  {
    public IStateDriver Resolve()
    {
      return new StateTestDriver(new StateTestDriverStore());
    }

    public void Boot()
    {

    }

    public void Stop()
    {

    }
  }
}
