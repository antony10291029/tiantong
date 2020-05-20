using System;
using System.Text;

namespace Tiantong.Iot.Protocol
{
  public class S7ReadResponse: IPlcReadResponse
  {
    private byte[] _message; 

    public byte[] Message
    {
      get => _message;
      set {
        _message = value;
        GetIsDataResponse();
        GetErrorCode();
        GetDataCode();
        GetDataLength();
        GetData();
      }
    }

    public bool IsDataResponse;

    public byte[] ErrorCode;

    public int DataLength;

    public byte DataCode;

    public byte[] Data;

    private int _length;

    public void UseBool()
    {
      throw new Exception("暂时不支持 Bool 类型");
    }

    public void UseUInt16()
    {
      _length = 2;
      Data = new byte[2];
    }

    public void UseInt32()
    {
      throw new Exception("暂时不支持 Int32 类型");
    }

    public void UseString(int length)
    {
      _length = length + 1;
      Data = new byte[_length];
    }

    public void UseBytes(int length)
    {
      throw new Exception("暂时不支持 Bytes 类型");
    }

    private void GetIsDataResponse()
    {
      IsDataResponse = Message[8] == 0x03;
    }

    private void GetErrorCode()
    {
      ErrorCode = new [] { Message[17], Message[18] };
      if (ErrorCode[0] != 0x00 || ErrorCode[1] != 0x00) {
        var errorCode = BitConverter.ToString(ErrorCode);

        throw new Exception($"错误类型、错误代码: {errorCode}");
      }
    }

    private void GetDataCode()
    {
      DataCode = Message[21];
    }

    private void GetDataLength()
    {
      DataLength = BitConverter.ToUInt16(new byte[] { Message[16], Message[15] }) - 4;
    }

    private void GetData()
    {
      Array.Copy(Message, 25, Data, 0, _length);
      if (DataLength != _length) {
        var byteString = BitConverter.ToString(Data);
        throw new Exception($"数据校验失败，长度应为: {_length}，实际长度: {DataLength}, 二进制数据: {byteString}");
      }
    }

    //

    public bool GetBool()
    {
      throw new Exception("暂时不支持 bool 类型");
    }

    public ushort GetUInt16()
    {
      return BitConverter.ToUInt16(Data);
    }

    public int GetInt32()
    {
      throw new Exception("暂时不支持 int32 类型");
    }

    public string GetString()
    {
      return Encoding.ASCII.GetString(Data[1..]);
    }

    public byte[] GetBytes()
    {
      return Data;
    }

  }
}
