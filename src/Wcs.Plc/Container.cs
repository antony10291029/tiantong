namespace Wcs.Plc
{
  public class Container: IContainer
  {
    static public IContainer GetTestContainer()
    {
      var container = new Container();

      container.StateDriver = new StateTestDriver();
      container.IntervalManager = new IntervalManager();      
      
      return container;
    }

    public IStateDriver StateDriver { get; set; }

    public IIntervalManager IntervalManager { get; set; }
  }
}
