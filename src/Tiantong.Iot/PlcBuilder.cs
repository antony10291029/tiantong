using System.Runtime.InteropServices.ComTypes;
using System;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public class PlcBuilder
  {
    public static PlcWorker Build(Plc plc)
    {
      var worker = new PlcWorker();

      worker.Config(configer => {
        configer.Id(plc.id).Name(plc.name)
          .Host(plc.host).Port(plc.port).Model(plc.model);
      });

      foreach (var state in plc.states) {
        var manager = worker.Define(state.name, state.id);
        switch (state.type) {
          case StateType.UInt16:
            manager.UInt16(state.address, StateBuilder(state));
            break;
          case StateType.String:
            manager.String(state.address, state.length, StateBuilder(state));
            break;
          default: throw new Exception($"不支持的 plc 数据类型: {state.type}");
        }
      }

      return worker;
    }

    public static Action<IState> StateBuilder(PlcState state)
    {
      return builder => {
        if (state.is_heartbeat) {
          builder.Heartbeat(state.heartbeat_interval, state.heartbeat_max_value);
        }

        if (state.is_collect) {
          builder.Collect(state.collect_interval);
        }

        foreach (var pusher in state.http_pushers) {
          builder.When(pusher.when_opt, pusher.when_value).Id(pusher.id)
            .HttpPost(pusher.url, pusher.value_key, pusher.to_string, pusher.data);
        }
      };
    }
  }
}