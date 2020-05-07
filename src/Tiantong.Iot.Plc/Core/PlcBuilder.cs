using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wcs.Plc.Entities;

namespace Tiantong.Iot.Plc
{
  public class PlcBuilder
  {
    private int _id;

    private string _name;

    private string _host;

    private int _port;

    private string _model;

    private StateBuilderResolver _stateResolver;

    //

    public List<StateBuilder> States;

    public PlcBuilder()
    {
      _stateResolver = new StateBuilderResolver(this);
    }

    public PlcBuilder Id(int id)
    {
      _id = id;

      return this;
    }

    public PlcBuilder Name(string name)
    {
      _name = name;

      return this;
    }

    public PlcBuilder Host(string host)
    {
      _host = host;

      return this;
    }

    public PlcBuilder Port(int port)
    {
      _port = port;

      return this;
    }

    public PlcBuilder Model(string model)
    {
      _model = model;

      return this;
    }

    public StateBuilderResolver State(string name)
    {
      return _stateResolver.Name(name);
    }

    public PlcRunner Build()
    {
      return new PlcRunner();
    }
  }

  public class PlcRunner
  {

  }

}
