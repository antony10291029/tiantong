using System;
using Wcs.Plc.Protocol;
using Renet.Tcp;

namespace Wcs.Plc
{
  public class StateRenetTcpClient : IStateClient
  {
    private IPlcReadRequest _readRequest;

    private IPlcReadResponse _readResponse;

    private IPlcWriteRequest _writeRequest;

    private IPlcWriteResponse _writeResponse;

    private RenetTcpClient _client;

    public StateRenetTcpClient(
      RenetTcpClient client,
      IPlcReadRequest readRequest,
      IPlcReadResponse readResponse,
      IPlcWriteRequest writeRequest,
      IPlcWriteResponse writeResponse
    ) {
      _client = client;
      _readRequest = readRequest;
      _readResponse = readResponse;
      _writeRequest = writeRequest;
      _writeResponse = writeResponse;
    }

    public void SetAddress(string key, int length)
    {
      _readRequest.UseAddress(key, length);
      _writeRequest.UseAddress(key, length);
    }

    public void SetBool(bool data)
    {
      _writeRequest.UseData(data);

      _writeResponse.Message = _client.TrySend(_writeRequest.Message);
    }

    public void SetInt(int data)
    {
      _writeRequest.UseData(data);

      _writeResponse.Message = _client.TrySend(_writeRequest.Message);
    }

    public void SetString(string data)
    {
      _writeRequest.UseData(data);
      _writeResponse.Message = _client.TrySend(_writeRequest.Message);
    }

    public bool GetBool()
    {
      _readResponse.Message = _client.TrySend(_readRequest.Message);

      return _readResponse.GetBool();
    }

    public int GetInt()
    {
      _readResponse.Message = _client.TrySend(_readRequest.Message);

      return _readResponse.GetInt32();
    }

    public string GetString()
    {
      _readResponse.Message = _client.TrySend(_readRequest.Message);

      return _readResponse.GetString();
    }
  }
}
