using System;
using System.Text;

namespace Wcs.Plc.Protocol
{
  public class MC3EReadResponse: IPlcReadResponse
  {
    private byte[] _message; 

    public byte[] Message
    {
      get => _message;
      set {
        _message = value;
        GetResultCode();
        GetErrorCode();
        GetData();
      }
    }

    private int _length;

    public byte[] ResultCode;

    public byte[] ErrorCode;

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
      throw new Exception("暂时不支持 Int32 类型");
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
        throw new Exception($"PLC读取错误，结束代码: {ResultCode}, 异常代码: {ErrorCode}");
      }
    }

    private void GetData()
    {
      Data = Message[11..(11 + _length)];
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
      if (Data.Length != 1) {
        throw new Exception("byte array length is not 1");
      }

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
