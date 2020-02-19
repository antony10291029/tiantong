using System;
using Wcs.Plc.Snap7;

namespace Wcs.Plc.Snap7
{
  public class S7Client : IStateClient
  {
    private S7Request _req;

    private S7Response _res;

    private S7TcpClient _client;

    public S7Client(S7TcpClient client)
    {
      _client = client;
      _req = new S7Request();
      _res = new S7Response();
    }

    public void SetAddress(string key, int length)
    {
      _req.UseAddress(key, length);
    }

    public void SetBool(bool data)
    {
      _req.UseData(data);
      _client.Write(_req, _res);
    }

    public void SetInt(int data)
    {
      _req.UseData(data);
      _client.Write(_req, _res);
    }

    public void SetString(string data)
    {
      _req.UseData(data);
      _client.Write(_req, _res);
    }

    public bool GetBool()
    {
      _client.Read(_req, _res);

      return _res.ToBool();
    }

    public int GetInt()
    {
      _client.Read(_req, _res);

      return _res.ToInt32();
    }

    public string GetString()
    {
      _client.Read(_req, _res);

      return _res.ToString();
    }
  }
}
