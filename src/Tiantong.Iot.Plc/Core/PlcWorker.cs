using System.Collections.Generic;

namespace Tiantong.Iot.Plc
{
  public class PlcWorker
  {
    private int _id;

    public Dictionary<int, State> States = new Dictionary<int, State>();

    public PlcWorker(Plc plc, List<State> states)
    {
      states.ForEach(state => States.Add(state.Id, state));
    }

  }
}
