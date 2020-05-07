namespace Tiantong.Iot.Plc
{
  public class StateBuilderResolver
  {
    private string _name;

    private PlcBuilder _plcBuilder;

    //

    private T AddState<T>(T state) where T : StateBuilder
    {
      _plcBuilder.States.Add(state);

      return state;
    }

    //

    public StateBuilderResolver(PlcBuilder builder)
    {
      _plcBuilder = builder;
    }

    public StateBuilderResolver Name(string name)
    {
      _name = name;

      return this;
    }

    public StateBuilder<ushort> UInt16(string address)
    {
      return AddState(new StateUInt16Builder().Name(_name).Address(address));
    }

    public StateBuilder<string> String(string address, int length)
    {
      return AddState(new StateStringBuilder().Name(_name).Address(address).Length(length));
    }
  }
}
