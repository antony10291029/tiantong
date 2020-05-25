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
            manager.UInt16(state.address, StateBuilder<ushort>(state, worker));
            break;
          case StateType.Int32:
            manager.Int32(state.address, StateBuilder<int>(state, worker));
            break;
          case StateType.String:
            manager.String(state.address, state.length, StateBuilder<string>(state, worker));
            break;
          default: throw new Exception($"不支持的 plc 数据类型: {state.type}");
        }
      }

      return worker;
    }

    public static Action<IState<T>> StateBuilder<T>(PlcState state, PlcWorker worker) => builder => {
      builder.PlcId(state.plc_id)
        .IsReadLogOn(state.is_read_log_on)
        .IsWriteLogOn(state.is_write_log_on);

      if (state.is_heartbeat) {
        worker.Heartbeat(state.name, state.heartbeat_interval, state.heartbeat_max_value);
      }

      if (state.is_collect) {
        worker.Collect<T>(state.name, state.collect_interval);
      }

      foreach (var pusher in state.http_pushers) {
        worker.HttpPusher<T>(state.name)
          .IsConcurrent(pusher.is_concurrent)
          .Post(pusher.url, pusher.value_key, pusher.is_value_to_string, pusher.data)
          .When(pusher.when_opt, pusher.when_value)
          .Id(pusher.id);
      }
    };

  }

}
