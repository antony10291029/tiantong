using System;
using System.Text;

namespace Tiantong.Iot.Protocol
{
  public class MC1EBinaryReadResponse: BinaryMessage, IPlcReadResponse
  {
    protected byte[] _message; 

    public override byte[] Message
    {
      get => _message;
      set {
        _message = value;
        GetResultCode();
        GetErrorCode();
      }
    }

    private int _dataLength;

    private int _messageDataLength;

    public byte[] ResultCode;

    public byte[] ErrorCode;

    public bool IsError
    {
      get => ResultCode[0] != 0x00;
    }

    public void UseBool()
    {
      _dataLength = _messageDataLength = 1;
    }

    public void UseInt16()
    {
      _dataLength = _messageDataLength = 2;
    }

    public void UseUInt16()
    {
      _dataLength = _messageDataLength = 2;
    }

    public void UseInt32()
    {
      _dataLength = _messageDataLength = 4;
    }

    public void UseUInt32()
    {
      _dataLength = _messageDataLength = 4;
    }

    public void UseString(int length)
    {
      _dataLength = _messageDataLength = length;

      if (_dataLength % 2 != 0) {
        _messageDataLength = _dataLength + 1;
      }
    }

    protected void GetResultCode()
    {
      ResultCode = Message[1..2];
    }

    protected void GetErrorCode()
    {
      if (IsError) {
        ErrorCode = Message[2..3];
        var errorCode = BitConverter.ToString(ErrorCode);
        var resultCode = BitConverter.ToString(ResultCode);
        throw KnownException.Error($"PLC 通信错误：结束代码: {resultCode}、异常代码: {errorCode}");
      }
    }

    //

    private byte[] GetData()
    {
      return Message[2..(2 + _dataLength)];
    }

    public bool GetBool()
    {
      return BitConverter.ToBoolean(GetData());
    }

    public ushort GetUInt16()
    {
      return BitConverter.ToUInt16(GetData());
    }

    public int GetInt32()
    {
      return BitConverter.ToInt32(GetData());
    }

    public int GetUInt32()
    {
      return BitConverter.ToInt32(GetData());
    }

    public string GetString()
    {
      return Encoding.ASCII.GetString(GetData());
    }
  }
}
