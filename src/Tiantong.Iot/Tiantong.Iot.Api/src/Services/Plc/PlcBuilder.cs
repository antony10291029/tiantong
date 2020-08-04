using System;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class PlcBuilder
  {
    private DomainContextFactory _domain;

    private HttpPusherClient _httpPusherClient;

    public PlcBuilder(
      DomainContextFactory domain,
      HttpPusherClient httpPusherClient
    ) {
      _domain = domain;
      _httpPusherClient = httpPusherClient;
    }

    public PlcClient BuildClient(Plc plc)
    {
      var options = new PlcClientOptions(plc.id, plc.name, plc.model, plc.host, plc.port, OnStateError);

      foreach (var st in plc.states) {
        IState state = st.type switch {
          PlcStateType.Bool => options.State<bool>(),
          PlcStateType.UInt16 => options.State<ushort>(),
          PlcStateType.Int32 => options.State<int>(),
          PlcStateType.String => options.State<string>(),
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
        ResolveState(client, manager, state);
      }

      return new PlcWorker(client, manager, _domain);
    }

    private void OnStateError(PlcStateError error)
    {
      _domain.Log(error);
    }

    private void ResolveState(PlcClient client, IntervalManager manager, PlcState st)
    {
      if (st.is_collect) {
        ResolveCollect(client, manager, st.name, st.collect_interval);
      }

      if (st.is_heartbeat) {
        ResolveHeartbeat(client, manager, st.name, st.heartbeat_interval, st.heartbeat_max_value);
      }

      foreach (var pusher in st.http_pushers) {
        ResolveHttpPusher(client, manager, st.name, pusher);
      }

      if (!st.is_collect && st.http_pushers?.Count > 0) {
        ResolveCollect(client, manager, st.name, 1000);
      }
    }

    private void ResolveCollect(PlcClient client, IntervalManager manager, string name, int interval)
    {
      manager.Add(new Interval(() => client.State(name).Collect(interval), interval));
    }

    private void ResolveHeartbeat(PlcClient client, IntervalManager manager, string name, int interval, int maxValue)
    {
      var value = 0;

      manager.Add(new Interval(() => {
        if (value < maxValue) {
          value = value + 1;
        } else {
          value = 1;
        }

        client.State(name).Set(value.ToString());
      }, interval));
    }

    private void ResolveHttpPusher(PlcClient client, IntervalManager manager, string name, HttpPusher pusher)
    {
      client.State(name).AddGetHook(async (value, oldValue) => {
        try {
          await _httpPusherClient.PostAsync(
            pusher.id,
            pusher.url,
            pusher.header,
            pusher.body,
            value,
            oldValue,
            pusher.field,
            System.Text.Encoding.UTF8
          );
        } catch {}
      });
    }
  }
}
