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
          case StateType.Int32:
            manager.Int32(state.address, StateBuilder(state));
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
        builder.PlcId(state.plc_id)
          .IsReadLogOn(state.is_read_log_on)
          .IsWriteLogOn(state.is_write_log_on);

        if (state.is_heartbeat) {
          builder.Heartbeat(state.heartbeat_interval, state.heartbeat_max_value);
        }

        if (state.is_collect) {
          builder.Collect(state.collect_interval);
        }

        foreach (var pusher in state.http_pushers) {
          builder.HttpPusher()
            .IsConcurrent(pusher.is_concurrent)
            .Post(pusher.url, pusher.value_key, pusher.is_value_to_string, pusher.data)
            .When(pusher.when_opt, pusher.when_value)
            .Id(pusher.id);
        }
      };
    }
  }
}
