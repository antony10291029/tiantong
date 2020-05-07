using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class HttpPusherRepository
  {
    private IotDbContext _db;

    public HttpPusherRepository(IotDbContext db)
    {
      _db = db;
    }

  }
}
