using System.Collections.Generic;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public interface IPlcWorkerLogger
  {
    void Log(int id, string message);
  }
}