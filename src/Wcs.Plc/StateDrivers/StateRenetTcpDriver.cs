using System;
using Wcs.Plc.Protocol;
using Renet.Tcp;

namespace Wcs.Plc
{
  public class StateTcpDriver : IStateDriver
  {
    private IPlcReadRequest _readRequest;

    private IPlcReadResponse _readResponse;

    private IPlcWriteRequest _writeRequest;

    private IPlcWriteResponse _writeResponse;

    private RenetTcpClient _client;

    public StateTcpDriver(
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

    private void Write()
    {
      _writeResponse.Message = _client.TrySend(_writeRequest.Message);
    }

    private void Read()
    {
      _readResponse.Message = _client.TrySend(_readRequest.Message);
    }

    public void UseBool()
    {
      _readRequest.UseBool();
      _readResponse.UseBool();
      _writeRequest.UseBool();
    }

    public void UseUInt16()
    {
      _readRequest.UseUInt16();
      _readResponse.UseUInt16();
      _writeRequest.UseUInt16();
    }

    public void UseInt32()
    {
      _readRequest.UseInt32();
      _readResponse.UseInt32();
      _writeRequest.UseInt32();
    }

    public void UseString(int length)
    {
      _readRequest.UseString(length);
      _readResponse.UseString(length);
      _writeRequest.UseString(length);
    }

    public void UseBytes(int length)
    {
      _readRequest.UseBytes(length);
      _readResponse.UseBytes(length);
      _writeRequest.UseBytes(length);
    }

    public void UseAddress(string key)
    {
      _readRequest.UseAddress(key);
      _writeRequest.UseAddress(key);
    }

    public void SetBool(bool data)
    {
      _writeRequest.UseData(data);
      Write();
    }

    public void SetUInt16(ushort data)
    {
      _writeRequest.UseData(data);
      Write();
    }

    public void SetInt32(int data)
    {
      _writeRequest.UseData(data);
      Write();
    }

    public void SetString(string data)
    {
      _writeRequest.UseData(data);
      Write();
    }

    public void SetBytes(byte[] data)
    {
      _writeRequest.UseData(data);
      Write();
    }

    public bool GetBool()
    {
      Read();

      return _readResponse.GetBool();
    }

    public ushort GetUInt16()
    {
      Read();

      return _readResponse.GetUInt16();
    }

    public int GetInt32()
    {
      Read();

      return _readResponse.GetInt32();
    }

    public string GetString()
    {
      Read();

      return _readResponse.GetString();
    }

    public byte[] GetBytes()
    {
      Read();

      return _readResponse.GetBytes();
    }
  }
}
