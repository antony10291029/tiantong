using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public interface ILogger
  {
    void UseDbContext(IotDbContext db);
  }

}
