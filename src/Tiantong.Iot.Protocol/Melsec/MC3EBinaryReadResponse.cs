using System;
using System.Text;

namespace Tiantong.Iot.Protocol
{
  public class MC3EBinaryReadResponse: BinaryMessage, IPlcReadResponse
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
      get => ResultCode[0] != 0x00 || ResultCode[1] != 0x00;
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
      ResultCode = Message[9..11];
    }

    protected void GetErrorCode()
    {
      if (IsError) {
        ErrorCode = Message[11..13];
        var errorCode = Encoding.ASCII.GetString(ErrorCode);
        var resultCode = Encoding.ASCII.GetString(ResultCode);
        throw new Exception($"结束代码: ${resultCode}, 异常代码: ${errorCode}");
      }
    }

    //

    private byte[] GetData()
    {
      var length = BitConverter.ToUInt16(Message[7..9]) - 2;

      if (length != _messageDataLength) {
        var bitString = BitConverter.ToString(Message[11..(11 + length)]);
        throw new Exception($"数据校对失败，长度应为：{_messageDataLength}，实际长度：{length}，二进制数据：{bitString}。");
      }

      return Message[11..(11 + _dataLength)];
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

    public bool GetBool()
    {
      return BitConverter.ToBoolean(GetData());
    }

    public string GetString()
    {
      return Encoding.ASCII.GetString(GetData());
    }

  }
}
