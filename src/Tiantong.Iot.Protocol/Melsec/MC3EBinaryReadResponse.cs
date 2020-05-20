using System;
using System.Text;

namespace Tiantong.Iot.Protocol
{
  public class MC3EBinaryReadResponse: IPlcReadResponse
  {
    private byte[] _message; 

    public byte[] Message
    {
      get => _message;
      set {
        _message = value;
        GetResultCode();
        GetErrorCode();
        GetDataLength();
        GetData();
      }
    }

    private int _length;

    public byte[] ResultCode;

    public byte[] ErrorCode;

    public int DataLength;

    public byte[] Data;

    public bool IsError
    {
      get => ResultCode[0] != 0x00 || ResultCode[1] != 0x00;
    }

    public void UseBool()
    {
      throw new Exception("暂时不支持 Bool 类型");
    }

    public void UseUInt16()
    {
      _length = 2;
    }

    public void UseInt32()
    {
      _length = 4;
    }

    public void UseString(int length)
    {
      _length = length;
    }

    public void UseBytes(int length)
    {
      throw new Exception("暂时不支持 Bytes 类型");
    }

    private void GetResultCode()
    {
      ResultCode = Message[9..11];
    }

    private void GetErrorCode()
    {
      if (IsError) {
        ErrorCode = Message[11..13];
        var errorCode = Encoding.ASCII.GetString(ErrorCode);
        var resultCode = Encoding.ASCII.GetString(ResultCode);
        throw new Exception($"结束代码: ${resultCode}, 异常代码: ${errorCode}");
      }
    }

    private void GetDataLength()
    {
      DataLength = BitConverter.ToUInt16(Message[7..9]) - 4;
    }

    private void GetData()
    {
      if (DataLength == 0) {
        return;
      }

      Data = Message[11..(11 + _length)];
      if (DataLength != _length) {
        var bitString = BitConverter.ToString(Data);
        throw new Exception($"数据校对失败，长度应为：{_length}，实际长度：{DataLength}，二进制数据：{bitString}。");
      }
    }

    //

    public ushort GetUInt16()
    {
      return BitConverter.ToUInt16(Data);
    }

    public int GetInt32()
    {
      return BitConverter.ToInt32(Data);
    }

    public bool GetBool()
    {
      return BitConverter.ToBoolean(Data);
    }

    public string GetString()
    {
      return Encoding.ASCII.GetString(Data);
    }

    public byte[] GetBytes()
    {
      return Data;
    }

  }
}
