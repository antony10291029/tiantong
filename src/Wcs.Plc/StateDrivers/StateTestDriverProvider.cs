namespace Wcs.Plc
{
  public class StateTestDriverProvider : IStateDriverProvider
  {
    public IStateDriver Resolve()
    {
      var driver = new StateTestDriver();
      var store = new StateTestDriverStore();

      driver.Store = store;

      return driver;
    }
  }
}
