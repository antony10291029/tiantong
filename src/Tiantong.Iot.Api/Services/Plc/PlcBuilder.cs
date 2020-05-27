using System;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class PlcBuilder
  {
    private DatabaseFactory _databaseFactory;

    private HttpPusherClient _httpPusherClient;

    private HttpPusherFactory _httpPusherFactory;

    public PlcBuilder(
      DatabaseFactory databaseFactory,
      HttpPusherClient httpPusherClient,
      HttpPusherFactory httpPusherFactory
    ) {
      _databaseFactory = databaseFactory;
      _httpPusherClient = httpPusherClient;
      _httpPusherFactory = httpPusherFactory;
    }

    public PlcClient BuildClient(Plc plc)
    {
      var options = new PlcClientOptions();

      options.Id(plc.id).Name(plc.name)
        .Host(plc.host).Port(plc.port).Model(plc.model)
        .OnStateError(OnStateError);

      foreach (var st in plc.states) {
        IState state = st.type switch {
          StateType.UInt16 => options.UInt16(),
          StateType.Int32 => options.Int(),
          StateType.String => options.String(),
          _ => throw new Exception($"暂不支持该数据类型: {st.type}")
        };

        state.Id(st.id).PlcId(st.plc_id).Name(st.name)
          .Address(st.address).Length(st.length)
          .IsReadLogOn(st.is_read_log_on).IsWriteLogOn(st.is_write_log_on);
      }

      return new PlcClient(options);
    }

    public PlcWorker BuildWorker(Plc plc)
    {
      var client = BuildClient(plc);
      var manager = new IntervalManager();

      foreach (var state in plc.states) {

        switch (state.type) {
          case StateType.UInt16:
            ResolveState<ushort>(client, manager, state);
            break;
          case StateType.Int32:
            ResolveState<int>(client, manager, state);
            break;
          case StateType.String:
            ResolveState<string>(client, manager, state);
            break;
          default:
            throw new Exception("暂时不支持该数据类型");
        };
      }

      return new PlcWorker(client, manager, _databaseFactory);
    }

    private void OnStateError(PlcStateError error)
    {
      _databaseFactory.Log(error);
    }

    private void ResolveState<T>(PlcClient client, IntervalManager manager, PlcState st)
    {
      if (st.is_collect) {
        ResolveCollect<T>(client, manager, st.name, st.collect_interval);
      }

      if (st.is_heartbeat) {
        ResolveHeartbeat(client, manager, st.name, st.heartbeat_interval, st.heartbeat_max_value);
      }

      foreach (var pusher in st.http_pushers) {
        ResolveHttpPusher<T>(client, manager, st.name)
          .IsConcurrent(pusher.is_concurrent)
          .Post(pusher.url, pusher.value_key, pusher.is_value_to_string, pusher.data)
          .When(pusher.when_opt, pusher.when_value)
          .Id(pusher.id);
      }
    }

    private void ResolveCollect<T>(PlcClient client, IntervalManager manager, string name, int interval)
    {
      Action handler = () => client.State<T>(name).Collect(interval);

      manager.Add(new Interval(handler, interval));
    }

    private void ResolveHeartbeat(PlcClient client, IntervalManager manager, string name, int interval, int maxValue)
    {
      var value = 1;

      Action handler = () => {
        if (value < maxValue) {
          value = value + 1;
        } else {
          value = 1;
        }

        client.State(name).SetString(value.ToString());
      };

      manager.Add(new Interval(handler, interval));
    }

    private HttpPusher<T> ResolveHttpPusher<T>(PlcClient client, IntervalManager manager, string name)
    {
      var pusher = _httpPusherFactory.Resolve<T>();

      client.State<T>(name).AddGetHook(value => pusher.Emit(value));

      return pusher;
    }

  }

}
