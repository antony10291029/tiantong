namespace Tiantong.Iot
{
  public class StateTestDriverProvider : IStateDriverProvider
  {
    private StateTestDriverStore _store = new StateTestDriverStore();

    public IStateDriver Resolve()
    {
      return new StateTestDriver(_store);
    }

    public void Connect()
    {

    }

    public void Close()
    {

    }
  }
}